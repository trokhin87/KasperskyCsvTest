using Application;
using Domain.Models;

namespace Infrastructure;

public class AggregationService:IAgregationService
{
    private readonly IStoreService _storeService;

    public AggregationService(IStoreService storeService)
    {
        _storeService = storeService;
    }
    public async Task<IEnumerable<(Severity severity, int count)>> AggregateBySeverityAsync(Guid Id)
    {
        var errorRecords = _storeService.Get(Id);
        if(errorRecords==null) return await Task.FromResult<IEnumerable<(Severity severity, int count)>>(null);
        var result = errorRecords.GroupBy(record => record.Severity)    
            .Select(g=>(g.Key,g.Count()))
            .OrderByDescending(x=>x.Item2);
        return await Task.FromResult(result);
    }

    


    public async Task<IEnumerable<(string product, string version, int count)>> AggregateByProductandVersionAsync(Guid Id)
    {
        var records = _storeService.Get(Id);
        if (records == null) return Enumerable.Empty<(string,string,int)>();
        var result=records
            .GroupBy(r=>new {r.Product ,r.Version})
            .Select(g =>
                (product : g.Key.Product,
                version : g.Key.Version,
                Count : g.Count())).
            OrderByDescending(x=>x.Count);
        return await Task.FromResult(result);
    }

    public async Task<IEnumerable<(DateTime Hour, string Product, string Version, string ErrorCode, int Count)>> MaxErrorCodePerHourAsync(Guid id)
    {
        var records = _storeService.Get(id);
        if (records == null) return await Task.FromResult<IEnumerable<(DateTime Hour, string Product, string Version, string ErrorCode, int Count)>>(null);
        var result = records
            .GroupBy(r => new 
            { 
                r.Product, 
                r.Version, 
                Hour = new DateTime(r.Timestamp.Year, r.Timestamp.Month, r.Timestamp.Day, r.Timestamp.Hour, 0, 0) 
            })
            .Select(g => g
                    .GroupBy(r => r.ErrorCode)
                    .Select(ecg => (
                        Hour : g.Key.Hour,
                        Product : g.Key.Product,
                        Version : g.Key.Version,
                        ErrorCode : ecg.Key,
                        Count : ecg.Count()
                    ))
                    .OrderByDescending(x => x.Count)
                    .First() // берём ErrorCode с максимальным количеством
            );
        return await Task.FromResult(result);
    }

    public async Task<IEnumerable<(string ErrorCode, int count)>> AggregateByErrorcodeAsync(Guid id, int top = 10)
    {
        var records = _storeService.Get(id);
        if (records == null) return await Task.FromResult<IEnumerable<(string ErrorCode, int Count)>>(null);
        var result=records
            .GroupBy(r=>new{r.Product ,r.Severity,r.ErrorCode})
            .Select(g=>(ErrorCode: g.Key.ErrorCode,Count:g.Count()))
            .OrderByDescending(x=>x.Count).Take(top);
        return await Task.FromResult(result);
    }

}