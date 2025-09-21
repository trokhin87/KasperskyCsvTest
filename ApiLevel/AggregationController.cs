using Application;
using Application.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiLevel;

/// <summary>
/// Контроллер для агрегации ошибок и экспорта результатов в CSV.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AggregationController : ControllerBase
{
    private readonly IAgregationService _aggregationService;
    private readonly ILogger<AggregationController> _logger;

    public AggregationController(IAgregationService agregationService, ILogger<AggregationController> logger)
    {
        _logger=logger;
        _aggregationService = agregationService;
    }

    [HttpGet("{id:guid}/severity")]
    [SwaggerOperation(
        Summary = "Агрегация по Severity",
        Description = "Возвращает общее количество ошибок, сгруппированных по Severity (Critical, High, Normal, Low)."
    )]
    [SwaggerResponse(200, "Успешно получены агрегированные данные", typeof(IEnumerable<SeverityAggregationDto>))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<SeverityAggregationDto>>> GetBySeverity(Guid id)
    {
        try
        {
            var result = await _aggregationService.AggregateBySeverityAsync(id);
            if (result == null || !result.Any())
            {
                _logger.LogWarning("Данные для id={id} не найдены (Severity)", id);
                return NotFound(new { Message = $"Данные для id={id} не найдены" });
            }

            _logger.LogInformation("Получены агрегированные данные Severity для id={id}", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при агрегации по Severity для id={id}", id);
            return StatusCode(500, new { Message = "Ошибка сервера", Error = ex.Message });
        }
    }

    [HttpGet("{id:guid}/productAndVersion")]
    [SwaggerOperation(
        Summary = "Агрегация по продуктам и версиям",
        Description = "Возвращает количество ошибок, сгруппированных по продукту и версии, отсортированных по убыванию."
    )]
    [SwaggerResponse(200, "Успешно получены агрегированные данные", typeof(IEnumerable<ProductVersionAggregationDto>))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<ProductVersionAggregationDto>>> GetByProductVersion(Guid id)
    {
        try
        {
            var result = await _aggregationService.AggregateByProductandVersionAsync(id);
            if (result == null || !result.Any())
            {
                _logger.LogWarning("Данные для id={id} не найдены (по продуктам и версиям)", id);
                return NotFound(new { Message = $"Данные для id={id} не найдены" });
            }

            _logger.LogInformation("Получены агрегированные данные по продуктам и версиям для id={id}", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при агрегации по продуктам и версиям для id={id}", id);
            return StatusCode(500, new { Message = "Ошибка сервера", Error = ex.Message });
        }
    }

    [HttpGet("{id:guid}/ErrorCode")]
    [SwaggerOperation(
        Summary = "Топ N ErrorCode",
        Description = "Возвращает топ N ошибок по ErrorCode, сгруппированных по продукту и Severity."
    )]
    [SwaggerResponse(200, "Успешно получены агрегированные данные", typeof(IEnumerable<ErrorCodeAggregationDto>))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<ErrorCodeAggregationDto>>> GetTopErrorCodes(
        Guid id,
        [FromQuery, SwaggerParameter("Количество записей в выборке (по умолчанию 10)", Required = false)] int top = 10)
    {
        try
        {
            var result = await _aggregationService.AggregateByErrorcodeAsync(id, top);
            if (result == null || !result.Any())
            {
                _logger.LogWarning("Данные для id={id} не найдены (ErrorCode)", id);
                return NotFound(new { Message = $"Данные для id={id} не найдены" });
            }

            _logger.LogInformation("Получены агрегированные данные по ErrorCode для id={id}", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при агрегации по ErrorCode для id={id}", id);
            return StatusCode(500, new { Message = "Ошибка сервера", Error = ex.Message });
        }
    }

    [HttpGet("{id:guid}/Time")]
    [SwaggerOperation(
        Summary = "Максимальный ErrorCode за каждый час",
        Description = "Возвращает самый частый ErrorCode для каждого часа по Product + Version."
    )]
    [SwaggerResponse(200, "Успешно получены агрегированные данные", typeof(IEnumerable<ErrorCodeHourAggregationDto>))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<ErrorCodeHourAggregationDto>>> GetMaxErrorCodePerHour(Guid id)
    {
        try
        {
            var result = await _aggregationService.MaxErrorCodePerHourAsync(id);
            if (result == null || !result.Any())
            {
                _logger.LogWarning("Данные для id={id} не найдены (ErrorCode)", id);
                return NotFound(new { Message = $"Данные для id={id} не найдены" });
            }

            _logger.LogInformation("Получены агрегированные данные Time для id={id}", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при агрегации по Time для id={id}", id);
            return StatusCode(500, new { Message = "Ошибка сервера", Error = ex.Message });
        }

    }

    // ---- Экспорт агрегаций ----

    [HttpGet("{id}/Severity/export")]
    [SwaggerOperation(
        Summary = "Экспорт агрегации по Severity",
        Description = "Сохраняет результаты агрегации по Severity в CSV файл."
    )]
    [SwaggerResponse(200, "Файл успешно сохранен")]
    public async Task<ActionResult> ExportSeverity(Guid id)
    {
        try
        {
            var data = await _aggregationService.AggregateBySeverityAsync(id);
            if (data == null || !data.Any())
            {
                _logger.LogWarning("Данные для id={id} не найдены (Severity)", id);
                return NotFound(new { Message = $"Данные для id={id} не найдены" });
            }

            var path = await _aggregationService.SaveAggregationToCsvAsync(data, $"severity_{id}.csv");
            _logger.LogInformation("Получен файл агрегированных данных Severity для id={id}", id);
            return Ok(new { Message = "Saved", Path = path });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при агрегации по Severity для id={id}", id);
            return StatusCode(500, new { Message = "Ошибка сервера", Error = ex.Message });
        }
    }

    [HttpGet("{id}/ProductVersion/export")]
    [SwaggerOperation(
        Summary = "Экспорт агрегации по продуктам и версиям",
        Description = "Сохраняет результаты агрегации по продуктам и версиям в CSV файл."
    )]
    [SwaggerResponse(200, "Файл успешно сохранен")]
    public async Task<ActionResult> ExportProductVersion(Guid id)
    {
        try
        {
            var data = await _aggregationService.AggregateByProductandVersionAsync(id);
            if (data == null || !data.Any())
            {
                _logger.LogWarning("Данные для id={id} не найдены (Product And Version)", id);
                return NotFound(new { Message = $"Данные для id={id} не найдены" });
            }
            
            var path = await _aggregationService.SaveAggregationToCsvAsync(data, $"product_version_{id}.csv");
            _logger.LogInformation("Получен файл агрегированных данных Severity для id={id}", id);
            return Ok(new { Message = "Saved", Path = path });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при агрегации по Product And Version для id={id}", id);
            return StatusCode(500, new { Message = "Ошибка сервера", Error = ex.Message });
        }
    }

    [HttpGet("{id}/ErrorCode/export")]
    [SwaggerOperation(
        Summary = "Экспорт топ N ErrorCode",
        Description = "Сохраняет результаты агрегации по ErrorCode в CSV файл."
    )]
    [SwaggerResponse(200, "Файл успешно сохранен")]
    public async Task<ActionResult> ExportErrorCodes(Guid id, [FromQuery] int top = 10)
    {
        try
        {
            var data = await _aggregationService.AggregateByErrorcodeAsync(id, top);
            if (data == null || !data.Any())
            {
                _logger.LogWarning("Данные для id={id} не найдены (ErrorCode)", id);
                return NotFound(new { Message = $"Данные для id={id} не найдены" });
            }
            var path = await _aggregationService.SaveAggregationToCsvAsync(data, $"errorcodes_{id}.csv");
            _logger.LogInformation("Получен файл агрегированных данных ErrorCode для id={id}", id);
            return Ok(new { Message = "Saved", Path = path });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при агрегации по ErrorCode для id={id}", id);
            return StatusCode(500, new { Message = "Ошибка сервера", Error = ex.Message });
        }
    }

    [HttpGet("{id}/MaxErrorCodePerHour/export")]
    [SwaggerOperation(
        Summary = "Экспорт максимального ErrorCode за каждый час",
        Description = "Сохраняет результаты агрегации (самый частый ErrorCode за каждый час) в CSV файл."
    )]
    [SwaggerResponse(200, "Файл успешно сохранен")]
    public async Task<ActionResult> ExportMaxErrorCodePerHour(Guid id)
    {
        try
        {
            var data = await _aggregationService.MaxErrorCodePerHourAsync(id);
            if (data == null || !data.Any())
            {
                _logger.LogWarning("Данные для id={id} не найдены (Time)", id);
                return NotFound(new { Message = $"Данные для id={id} не найдены" });
            }
            var path = await _aggregationService.SaveAggregationToCsvAsync(data, $"max_errorcode_per_hour_{id}.csv");
            _logger.LogInformation("Получен файл агрегированных данных Time для id={id}", id);
            return Ok(new { Message = "Saved", Path = path });
        }       
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при агрегации по Time для id={id}", id);
            return StatusCode(500, new { Message = "Ошибка сервера", Error = ex.Message });
        }
    }
}
