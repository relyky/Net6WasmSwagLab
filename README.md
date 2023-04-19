# Net6WasmSwagLab
.NET6  + WASM + Swagger with NSwag   

# 目的
在 Blazor WebAssembly 開發中，預設與後台的通訊方法用 HttpClient，這很遜，這不是微軟該有的品質。   
連叫用本身網站後端的 Web API 都要繞一大圈，這很遜，這不是微軟該有的品質。   
叫用本身網站後端的 Web API 應該直接呼叫就行了，這才是微軟該有的品質。   
在評估後應該可以把本身後端的 Web API 包裝成 Swagger API，其前端的碼可以自動生成對應的 swagClient。

# 環境
* .NET6
* Blazor WASM APP
* Visual Studio 2022
  * 預設的 OpenAPI 服務參考產生的碼只能用在最簡單的情境，~就是不敷使用~。
  * 延伸模組： `Unchase OpenAPI (Swagger) Connected Service VS2022`
      * for 生成相應的 Swagger client code。
      * 生成的碼與 NSwagStudio 是一樣的，只是版本可能落後一點點。
      * 設定好後可以自動更新 Swagger client code。 
      * `Unchase OpenAPI (Swagger) Connected Service VS2022` 設定參數與 NSwagStudio 一模一樣。
* `NSwag.AspNetCore v13.18.2`   
  * [Get started with NSwag and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-nswag?view=aspnetcore-6.0&tabs=visual-studio)
  * for Swagger server side 產生 Swagger 網頁與 Swaggger 描述檔 `swagger.json / swagger.nswag`。

## Unchase OpenAPI (Swagger) Connected Service VS2022 要點
 * [Unchase OpenAPI (Swagger) Connected Service VS2022](https://marketplace.visualstudio.com/items?itemName=Unchase.unchaseopenapiconnectedservicevs22)
 * 為 Visual Studio 2022 的延伸模組
 * for 生成相應的 Swagger client code。
 * 其核心是 NSwag。
 * 生成的碼與 NSwagStudio 是一樣的，只是版本可能落後一點點。
 * 設定好後可以自動更新 Swagger client code。 
 * `Unchase OpenAPI (Swagger) Connected Service VS2022` 參數設定與 NSwagStudio 一模一樣。

## NSwagStudio 工具程式(進階)  
  * 可生成最新版且最完整的 Swagger client code。
  * 缺點是全手工作業。
  * 若要設定後可直接自動更新就要用 `Unchase OpenAPI (Swagger) Connected Service VS2022` 延伸模組，其設定參數與 NSwagStudio 一模一樣。

# Swashbuckle vs Nswag
這是本人之前的比較紀錄 [Swagger UI Relay Lab](https://github.com/relyky/Swagger-UI-Relay-Lab)。   
Swashbuckle 仍只支援後端的部份，整合性高一點點，但因不支援前端所以分數加不上去。  
NSwag 在後端、前端都有支援且 NSwag 也支授 .NET6 了所以 NSwag 勝出。   
以現在的時間點來說 Swashbuckle 還是很好用的。

# 小小瑕疵
在此時此版 NSwag 的 JSON 序列化仍必需依賴 `Newtonsoft.Json`。

# NSwagStudio 設定紀錄
參考這裡[NSwagStudio 使用紀錄](https://rely-ky.gitbook.io/qu-zhi-wu-wang-lu-gitbook2/nswagstudio-shi-yong-ji-lu)

# 關鍵原碼紀錄
## Swagger Server code sample 
``` csharp
// File: Server/Controllers/TodoController.cs

using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Net6WasmSwagLab.DTO;

namespace Net6WasmSwagLab.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
  List<TodoDto> _simsTodoRepo = new()
  {
      new() { Sn = 1, Description = "今天天氣真好", Done = false, CreateDtm = DateTime.Now.AddDays(-3) },
      new() { Sn = 2, Description = "下午去看電影", Done = false, CreateDtm = DateTime.Now.AddDays(-2) },
      new() { Sn = 3, Description = "晚上吃大餐", Done = false, CreateDtm = DateTime.Now.AddDays(-1) }
  };
  
  ...省略...

  [HttpPost("[action]")]
  [SwaggerResponse(200, typeof(List<TodoDto>))]
  [SwaggerResponse(400, typeof(ErrMsg))]
  public IActionResult QryDataList(TodoQryAgs args)
  {
    if (args.Msg == "測試邏輯失敗")
    {
      return BadRequest(new ErrMsg("這是邏輯失敗！"));
    }

    return Ok(_simsTodoRepo);
  }
}

```

## Blaozr client sample
``` csharp
// File: Client/Pages/TodoLab.razor

@page "/todo"
@inject SwagClient.TodoApi bizApi //------ 注入 Swagger API client

<PageTitle>Todo list</PageTitle>
...省略(render page)...

@code {
  List<TodoDto>? todoList = null;
  string errMsg = string.Empty;
  bool f_testFail = false;

  protected override async Task OnInitializedAsync()
  {
    await HandleQuery();
  }

  async Task HandleQuery()
  {
    try
    {
      errMsg = string.Empty;
      todoList = null;

      var args = new TodoQryAgs {
        Msg = f_testFail ? "測試邏輯失敗" : "今天天氣真好",
        Amt = 999
      };

      todoList = await bizApi.QryDataListAsync(args); //------ 叫用 Swagger API
    }
    catch (SwagClient.ApiException<ErrMsg> ex)
    {
      // 邏輯失敗!
      errMsg = $"ApiException: {ex.Result.Severity}-{ex.Result.Message}";
    }
    catch (Exception ex)
    {
      // 例外失敗!
      errMsg = "EXCEPTION: " + ex.Message;
    }
  }
}
```

## Swagger client code sample
請直接參考原始碼:
>
> Client/Connected Services/OpenAPIService/OpenAPI.cs
>

## 註冊 SeagClient 
註冊成 DI service 以可以注入(@inject)。  
``` csharp
// File: Client/Program.cs

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Net6WasmSwagLab.Client;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//## 註冊 SwagClient API。 --- 相關 SwagClient 資源備好後才能在此註冊
var asm = Assembly.GetAssembly(typeof(App));
foreach (var swagClientType in asm.GetTypes().Where(c => c.Namespace == "SwagClient" && c.Name.EndsWith("Api")))
{
  builder.Services.AddScoped(swagClientType, provider =>
  {
    var http = provider.GetRequiredService<HttpClient>();
    var baseUrl = builder.HostEnvironment.BaseAddress;
    var swagClient = Activator.CreateInstance(swagClientType, http);
    swagClientType.GetProperty("BaseUrl")?.SetValue(swagClient, baseUrl);
    return swagClient;

    //※ 需考慮引入的 NSwag 版本而調整。此例是["NSwag", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.1.0))")]
    //## 等同生成下方程式碼實體
    // var swagClient = new SwagClient.WeatherForecastApi(http);
    // swagClient.BaseUrl = baseUrl;
  });
}

await builder.Build().RunAsync();
```
