using Application;
using Infrastructure;
using Infrastructure.GenerateErrors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSingleton<IGenerateTime, GenerateTime>();
builder.Services.AddSingleton<IGenerateSeverity, GenerateSeverity>();
builder.Services.AddSingleton<IGenerateProduct, GenerateProduct>();
builder.Services.AddSingleton<IGenerateVersion, GenerateVersion>();
builder.Services.AddSingleton<IGenerateErrorCode, GenerateErrorCode>();
builder.Services.AddSingleton<IErorRecordGenerator, ErrorRecordGenerator>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();
