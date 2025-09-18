using Application;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiLevel;

[ApiController]
[Route("api/[controller]")]
public class GenerateController:ControllerBase
{
    private readonly IGeneratorService _generatorService;

    public GenerateController(IGeneratorService generatorService)
    {
        _generatorService = generatorService;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Генерация CSV файла",
        Description = "Создает CSV-файл с 10 000 случайных ошибок (Timestamp, Severity, Product, Version, ErrorCode)"
    )]
    [SwaggerResponse(200, "Файл успешно сгенерирован и доступен для анализа")]
    public async Task<IActionResult> Generate(int count = 10000)
    {
        var id = await _generatorService.GenerateAsync(count);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", $"{id}.csv");
        return Ok(new { Id = id,File=filePath, Message = $"{count} records generated" });
    }
}