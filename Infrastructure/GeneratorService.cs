using Application;
using Domain.Models;

namespace Infrastructure;

public class GeneratorService:IGeneratorService
{
    private readonly IErorRecordGenerator _generator;
    private readonly IStoreService _storeService;
    private readonly CsvFileService _csvService;


    public GeneratorService(IErorRecordGenerator generator, IStoreService storeService,CsvFileService csvService)
    {
        _generator = generator;
        _storeService = storeService;
        _csvService = csvService;
    }
    public async Task<Guid> GenerateAsync(int count = 10000)
    {
        var records = new List<ErrorRecord>(count);
        for (int i = 0; i < count; i++)
        {
            records.AddRange(_generator.Generate());
        }

        var id = _storeService.Save(records);
        var path=_csvService.Save(records,id);
        return await Task.FromResult(id);
    }
}