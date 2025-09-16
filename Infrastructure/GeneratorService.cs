using Application;
using Domain.Models;

namespace Infrastructure;

public class GeneratorService:IGeneratorService
{
    private readonly IErorRecordGenerator _generator;
    private readonly IStoreService _storeService;

    public GeneratorService(IErorRecordGenerator generator, IStoreService storeService)
    {
        _generator = generator;
        _storeService = storeService;
    }
    public async Task<Guid> GenerateAsync(int count = 10000)
    {
        var records = new List<ErrorRecord>(count);
        for (int i = 0; i < count; i++)
        {
            records.Add(_generator.Generate());
        }

        var id = _storeService.Save(records);
        return await Task.FromResult(id);
    }
}