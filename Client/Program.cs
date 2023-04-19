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
