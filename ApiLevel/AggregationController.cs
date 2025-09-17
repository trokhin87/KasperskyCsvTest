using Application;
using Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ApiLevel;
[ApiController]
[Route("api/[controller]")]
public class AggregationController:ControllerBase
{
    private readonly IAgregationService _aggregationService;

    public AggregationController(IAgregationService agregationService)
    {
        _aggregationService = agregationService;
    }
    [HttpGet("{id:guid}/severity")]
    public async Task<ActionResult<IEnumerable<SeverityAggregationDto>>> GetBySeverity(Guid id)
    {
        var result = await _aggregationService.AggregateBySeverityAsync(id);
        return Ok(result);
    }
    
    [HttpGet("{id:guid}/productAndVersion")]
    public async Task<ActionResult<IEnumerable<ProductVersionAggregationDto>>> GetByProductVersion(Guid id)
    {
        var result = await _aggregationService.AggregateByProductandVersionAsync(id);
        return Ok(result);
    }


    [HttpGet("{id:guid}/ErrorCode")]
    public async Task<ActionResult<IEnumerable<ErrorCodeAggregationDto>>> GetTopErrorCodes(Guid id, [FromQuery] int top = 10)
    {
        var result = await _aggregationService.AggregateByErrorcodeAsync(id, top);
        return Ok(result);
    }
    
    [HttpGet("{id:guid}/Time")]
    public async Task<ActionResult<IEnumerable<ErrorCodeHourAggregationDto>>> GetMaxErrorCodePerHour(Guid id)
    {
        var result = await _aggregationService.MaxErrorCodePerHourAsync(id);
        return Ok(result);
    }
    
    
    
    
    
    
    
    
    
    [HttpGet("{id}/Severity/export")]
    public async Task<ActionResult> ExportSeverity(Guid id)
    {
        var data = await _aggregationService.AggregateBySeverityAsync(id);
        var path = await _aggregationService.SaveAggregationToCsvAsync(data, $"severity_{id}.csv");
        return Ok(new { Message = "Saved", Path = path });
    }

    [HttpGet("{id}/ProductVersion/export")]
    public async Task<ActionResult> ExportProductVersion(Guid id)
    {
        var data = await _aggregationService.AggregateByProductandVersionAsync(id);
        var path = await _aggregationService.SaveAggregationToCsvAsync(data, $"product_version_{id}.csv");
        return Ok(new { Message = "Saved", Path = path });
    }

    [HttpGet("{id}/ErrorCode/export")]
    public async Task<ActionResult> ExportErrorCodes(Guid id, [FromQuery] int top = 10)
    {
        var data = await _aggregationService.AggregateByErrorcodeAsync(id, top);
        var path = await _aggregationService.SaveAggregationToCsvAsync(data, $"errorcodes_{id}.csv");
        return Ok(new { Message = "Saved", Path = path });
    }

    [HttpGet("{id}/MaxErrorCodePerHour/export")]
    public async Task<ActionResult> ExportMaxErrorCodePerHour(Guid id)
    {
        var data = await _aggregationService.MaxErrorCodePerHourAsync(id);
        var path = await _aggregationService.SaveAggregationToCsvAsync(data, $"max_errorcode_per_hour_{id}.csv");
        return Ok(new { Message = "Saved", Path = path });
    }
}