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

# 小小瑕疵
在此時此版 NSwag 的 JSON 序列化仍必需依賴 `Newtonsoft.Json`。


