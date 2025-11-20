using Lab13_BryanPumacayo.Data;
using Lab13_BryanPumacayo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ReportsRepository>();
builder.Services.AddScoped<ExcelExampleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lab13-BryanPumacayo API v1");
        c.RoutePrefix = string.Empty; 
    });
}

app.MapGet("/excel/first-example", (ExcelExampleService service) =>
    {
        var path = service.FirstExample();
        return Results.Ok(new
        {
            message = "Archivo Excel creado correctamente.",
            filePath = path
        });
    })
    .WithName("FirstExample");

app.MapGet("/excel/second-example", (ExcelExampleService service) =>
    {
        var path = service.SecondExample();
        return Results.Ok(new
        {
            message = "Archivo Excel modificado correctamente.",
            filePath = path
        });
    })
    .WithName("SecondExample");

app.MapGet("/excel/third-example", (ExcelExampleService service) =>
    {
        var path = service.ThirdExample();
        return Results.Ok(new
        {
            message = "Archivo Excel con tabla creado correctamente.",
            filePath = path
        });
    })
    .WithName("ThirdExample");

app.MapGet("/excel/fourth-example", (ExcelExampleService service) =>
    {
        var path = service.FourthExample();
        return Results.Ok(new
        {
            message = "Archivo Excel con estilos creado correctamente.",
            filePath = path
        });
    })
    .WithName("FourthExample");


app.MapGet("/reports/clients-summary", async (ExcelExampleService service) =>
    {
        var path = await service.GenerateClientsReportAsync();
        return Results.Ok(new
        {
            message = "Reporte de clientes generado correctamente.",
            filePath = path
        });
    })
    .WithName("ClientsSummaryReport");

app.MapGet("/reports/products-summary", async (ExcelExampleService service) =>
    {
        var path = await service.GenerateProductsReportAsync();
        return Results.Ok(new
        {
            message = "Reporte de productos generado correctamente.",
            filePath = path
        });
    })
    .WithName("ProductsSummaryReport");

app.Run();
