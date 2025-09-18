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

    public AggregationController(IAgregationService agregationService)
    {
        _aggregationService = agregationService;
    }

    [HttpGet("{id:guid}/severity")]
    [SwaggerOperation(
        Summary = "Агрегация по Severity",
        Description = "Возвращает общее количество ошибок, сгруппированных по Severity (Critical, High, Normal, Low)."
    )]
    [SwaggerResponse(200, "Успешно получены агрегированные данные", typeof(IEnumerable<SeverityAggregationDto>))]
    public async Task<ActionResult<IEnumerable<SeverityAggregationDto>>> GetBySeverity(Guid id)
    {
        var result = await _aggregationService.AggregateBySeverityAsync(id);
        return Ok(result);
    }

    [HttpGet("{id:guid}/productAndVersion")]
    [SwaggerOperation(
        Summary = "Агрегация по продуктам и версиям",
        Description = "Возвращает количество ошибок, сгруппированных по продукту и версии, отсортированных по убыванию."
    )]
    [SwaggerResponse(200, "Успешно получены агрегированные данные", typeof(IEnumerable<ProductVersionAggregationDto>))]
    public async Task<ActionResult<IEnumerable<ProductVersionAggregationDto>>> GetByProductVersion(Guid id)
    {
        var result = await _aggregationService.AggregateByProductandVersionAsync(id);
        return Ok(result);
    }

    [HttpGet("{id:guid}/ErrorCode")]
    [SwaggerOperation(
        Summary = "Топ N ErrorCode",
        Description = "Возвращает топ N ошибок по ErrorCode, сгруппированных по продукту и Severity."
    )]
    [SwaggerResponse(200, "Успешно получены агрегированные данные", typeof(IEnumerable<ErrorCodeAggregationDto>))]
    public async Task<ActionResult<IEnumerable<ErrorCodeAggregationDto>>> GetTopErrorCodes(
        Guid id,
        [FromQuery, SwaggerParameter("Количество записей в выборке (по умолчанию 10)", Required = false)] int top = 10)
    {
        var result = await _aggregationService.AggregateByErrorcodeAsync(id, top);
        return Ok(result);
    }

    [HttpGet("{id:guid}/Time")]
    [SwaggerOperation(
        Summary = "Максимальный ErrorCode за каждый час",
        Description = "Возвращает самый частый ErrorCode для каждого часа по Product + Version."
    )]
    [SwaggerResponse(200, "Успешно получены агрегированные данные", typeof(IEnumerable<ErrorCodeHourAggregationDto>))]
    public async Task<ActionResult<IEnumerable<ErrorCodeHourAggregationDto>>> GetMaxErrorCodePerHour(Guid id)
    {
        var result = await _aggregationService.MaxErrorCodePerHourAsync(id);
        return Ok(result);
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
        var data = await _aggregationService.AggregateBySeverityAsync(id);
        var path = await _aggregationService.SaveAggregationToCsvAsync(data, $"severity_{id}.csv");
        return Ok(new { Message = "Saved", Path = path });
    }

    [HttpGet("{id}/ProductVersion/export")]
    [SwaggerOperation(
        Summary = "Экспорт агрегации по продуктам и версиям",
        Description = "Сохраняет результаты агрегации по продуктам и версиям в CSV файл."
    )]
    [SwaggerResponse(200, "Файл успешно сохранен")]
    public async Task<ActionResult> ExportProductVersion(Guid id)
    {
        var data = await _aggregationService.AggregateByProductandVersionAsync(id);
        var path = await _aggregationService.SaveAggregationToCsvAsync(data, $"product_version_{id}.csv");
        return Ok(new { Message = "Saved", Path = path });
    }

    [HttpGet("{id}/ErrorCode/export")]
    [SwaggerOperation(
        Summary = "Экспорт топ N ErrorCode",
        Description = "Сохраняет результаты агрегации по ErrorCode в CSV файл."
    )]
    [SwaggerResponse(200, "Файл успешно сохранен")]
    public async Task<ActionResult> ExportErrorCodes(Guid id, [FromQuery] int top = 10)
    {
        var data = await _aggregationService.AggregateByErrorcodeAsync(id, top);
        var path = await _aggregationService.SaveAggregationToCsvAsync(data, $"errorcodes_{id}.csv");
        return Ok(new { Message = "Saved", Path = path });
    }

    [HttpGet("{id}/MaxErrorCodePerHour/export")]
    [SwaggerOperation(
        Summary = "Экспорт максимального ErrorCode за каждый час",
        Description = "Сохраняет результаты агрегации (самый частый ErrorCode за каждый час) в CSV файл."
    )]
    [SwaggerResponse(200, "Файл успешно сохранен")]
    public async Task<ActionResult> ExportMaxErrorCodePerHour(Guid id)
    {
        var data = await _aggregationService.MaxErrorCodePerHourAsync(id);
        var path = await _aggregationService.SaveAggregationToCsvAsync(data, $"max_errorcode_per_hour_{id}.csv");
        return Ok(new { Message = "Saved", Path = path });
    }
}
