using Application;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Generate(int count = 10000)
    {
        var id = await _generatorService.GenerateAsync(count);
        return Ok(new { Id = id, Message = $"{count} records generated" });
    }
}