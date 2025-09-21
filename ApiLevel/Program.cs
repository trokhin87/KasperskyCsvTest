using Application;
using Infrastructure;
using Infrastructure.GenerateErrors;
using Infrastructure.Stores;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});
// builder.Logging.ClearProviders();
// builder.Logging.AddConsole();
// builder.Logging.AddDebug();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning) 
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .WriteTo.File(
        path: "logss/log-.txt",                     
        rollingInterval: RollingInterval.Day,      
        retainedFileCountLimit: 7,                 
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();
builder.Host.UseSerilog();



var date = DateOnly.FromDateTime(DateTime.Now);
builder.Services.AddSingleton<IGenerateTime>(sp=>new GenerateTime(date));
builder.Services.AddSingleton<IGenerateSeverity, GenerateSeverity>();
builder.Services.AddSingleton<IGenerateProduct, GenerateProduct>();
builder.Services.AddSingleton<IGenerateVersion, GenerateVersion>();
builder.Services.AddSingleton<IGenerateErrorCode, GenerateErrorCode>();
builder.Services.AddSingleton<IErorRecordGenerator, ErrorRecordGenerator>();

builder.Services.AddSingleton<IStoreService, StoreService>();
builder.Services.AddSingleton<IGeneratorService,GeneratorService >();
builder.Services.AddSingleton<IAgregationService, AggregationService>();

builder.Services.AddSingleton<CsvFileService>();


builder.Services.AddControllers();




var app = builder.Build();
app.Use(async (context, next) =>
{
    var sw = System.Diagnostics.Stopwatch.StartNew();
    await next();
    sw.Stop();

    Log.Information(
        "HTTP {Method} {Path} → {StatusCode} за {Elapsed:0.000} сек",
        context.Request.Method,
        context.Request.Path,
        context.Response.StatusCode,
        sw.Elapsed.TotalSeconds
    );
});


app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();
