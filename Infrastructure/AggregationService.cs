using Application;
using Application.DTO;
using Domain.Models;

namespace Infrastructure;

public class AggregationService:IAgregationService
{
    private readonly IStoreService _storeService;
    private readonly CsvFileService _csvFileService;

    public AggregationService(IStoreService storeService, CsvFileService csvFileService)
    {
        _storeService = storeService;
        _csvFileService = csvFileService;
    }

    public async Task<IEnumerable<SeverityAggregationDto>> AggregateBySeverityAsync(Guid Id)
    {
        var errorRecords = _storeService.Get(Id);
        if (errorRecords == null) return Enumerable.Empty<SeverityAggregationDto>();

        var result = errorRecords
            .GroupBy(r => r.Severity)
            .Select(g => new SeverityAggregationDto(g.Key.ToString(), g.Count()))
            .OrderByDescending(x => x.Count);

        return await Task.FromResult(result);
    }

    public async Task<IEnumerable<ProductVersionAggregationDto>> AggregateByProductandVersionAsync(Guid Id)
    {
        var records = _storeService.Get(Id);
        if (records == null) return Enumerable.Empty<ProductVersionAggregationDto>();

        var result = records
            .GroupBy(r => new { r.Product, r.Version })
            .Select(g => new ProductVersionAggregationDto(g.Key.Product, g.Key.Version, g.Count()))
            .OrderByDescending(x => x.Count);

        return await Task.FromResult(result);
    }

    public async Task<IEnumerable<ErrorCodeHourAggregationDto>> MaxErrorCodePerHourAsync(Guid id)
    {
        var records = _storeService.Get(id);
        if (records == null) return Enumerable.Empty<ErrorCodeHourAggregationDto>();

        var result = records
            .GroupBy(r => new
            {
                r.Product,
                r.Version,
                Hour = new DateTime(r.Timestamp.Year, r.Timestamp.Month, r.Timestamp.Day, r.Timestamp.Hour, 0, 0)
            })
            .Select(g => g
                .GroupBy(r => r.ErrorCode)
                .Select(ecg => new ErrorCodeHourAggregationDto(
                    g.Key.Hour,
                    g.Key.Product,
                    g.Key.Version,
                    ecg.Key,
                    ecg.Count()
                ))
                .OrderByDescending(x => x.Count)
                .First()
            );

        return await Task.FromResult(result);
    }

    public async Task<IEnumerable<ErrorCodeAggregationDto>> AggregateByErrorcodeAsync(Guid id, int top = 10)
    {
        var records = _storeService.Get(id);
        if (records == null) return Enumerable.Empty<ErrorCodeAggregationDto>();

        var result = records
            .GroupBy(r => r.ErrorCode)
            .Select(g => new ErrorCodeAggregationDto(g.Key, g.Count()))
            .OrderByDescending(x => x.Count)
            .Take(top);

        return await Task.FromResult(result);
    }
    
    
    public async Task<string> SaveAggregationToCsvAsync<T>(IEnumerable<T> aggregation, string fileName)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Aggregations", fileName);

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        await _csvFileService.SaveToCsvAsync(aggregation, path);

        return path;
    }

}