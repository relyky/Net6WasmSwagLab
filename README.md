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
  * 延伸模組： `Unchase OpenAPI (Swagger) Connected Service VS2022`
      * for 生成相應的 Swagger client code。
      * 生成的碼與 NSwagStudio 是一樣的，只是版本可能落後一點點。
      * 設定好後可以自動更新 Swagger client code。 
      * `Unchase OpenAPI (Swagger) Connected Service VS2022` 設定參數與 NSwagStudio 一模一樣。
* Blazor WASM APP
* NSwag.AspNetCore v13.18.2   
  * for Swagger server side 產生 Swagger 網頁

## NSwagStudio 工具程式(進階)  
  * 可生成最新版且最完整的 Swagger client code。
  * 缺點是全手工作業。
  * 若要設定後可直接自動更新就要用 `Unchase OpenAPI (Swagger) Connected Service VS2022` 延伸模組，其設定參數與 NSwagStudio 一模一樣。

# Swashbuckle vs Nswag

參考

# 小小瑕疵
在此時此版 NSwag 的 JSON 序列化仍必需依賴 `Newtonsoft.Json`。


