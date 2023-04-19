using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Net6WasmSwagLab.Client;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//## ���U SwagClient API�C --- ���� SwagClient �귽�Ʀn��~��b�����U
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

    //�� �ݦҼ{�ޤJ�� NSwag �����ӽվ�C���ҬO["NSwag", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.1.0))")]
    //## ���P�ͦ��U��{���X����
    // var swagClient = new SwagClient.WeatherForecastApi(http);
    // swagClient.BaseUrl = baseUrl;
  });
}

await builder.Build().RunAsync();
