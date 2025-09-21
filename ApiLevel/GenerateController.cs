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
    private readonly ILogger<GenerateController> _logger;

    public GenerateController(IGeneratorService generatorService, ILogger<GenerateController> logger)
    {
        _logger = logger;
        _generatorService = generatorService;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Генерация CSV файла",
        Description = "Создает CSV-файл с 10 000 случайных ошибок (Timestamp, Severity, Product, Version, ErrorCode)"
    )]
    [SwaggerResponse(200, "Файл успешно сгенерирован и доступен для анализа")]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Generate(int count = 10000)
    {
        if (count <= 0)
        {
            _logger.LogWarning("Некорректный параметр count={count}", count);
            return BadRequest(new { Message = "Количество должно быть больше 0" });
        }

        try
        {
            var id = await _generatorService.GenerateAsync(count);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", $"{id}.csv");

            _logger.LogInformation("Сгенерирован CSV на {count} строк, id={id}, path={file}", count, id, filePath);

            return Ok(new { Id = id, File = filePath, Message = $"{count} records generated" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при генерации CSV файла (count={count})", count);
            return StatusCode(500, new { Message = "Ошибка сервера", Error = ex.Message });
        }
    }
}