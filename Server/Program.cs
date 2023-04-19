using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register the Swagger services
builder.Services.AddSwaggerDocument(config =>
{
  config.PostProcess = document =>
  {
    document.Info.Version = "v1";
    document.Info.Title = "我的 Swagger API 練習";
    document.Info.Description = "我的 Swagger API 練習。使用 NSwag 生成。";
    //document.Info.TermsOfService = "None";
    //document.Info.Contact = new NSwag.OpenApiContact
    //{
    //  Name = "Asia Vista Technology Co., Ltd.",
    //  Email = string.Empty,
    //  Url = "https://www.asiavista.com.tw/"
    //};
    //document.Info.License = new NSwag.OpenApiLicense
    //{
    //  Name = "MIT",
    //  Url = "https://opensource.org/license/mit/"
    //};
  };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseWebAssemblyDebugging();
}
else
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

// Register the Swagger generator and the Swagger UI middlewares
app.UseOpenApi();
app.UseSwaggerUi3();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
