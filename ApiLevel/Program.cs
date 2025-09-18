using Application;
using Infrastructure;
using Infrastructure.GenerateErrors;
using Infrastructure.Stores;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});
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
