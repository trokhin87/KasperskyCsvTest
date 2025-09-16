using Domain.Models;

namespace Application;

public interface IAgregationService
{
    Task<IEnumerable<(Severity severity,int count)>>AggregateBySeverityAsync(Guid Id);
    Task<IEnumerable<(string product,string version,int count)>>AggregateByProductandVersionAsync(Guid Id);
    Task<IEnumerable<(DateTime Hour, string Product, string Version, string ErrorCode, int Count)>> MaxErrorCodePerHourAsync(Guid id);
    Task<IEnumerable<(string ErrorCode, int count)>> AggregateByErrorcodeAsync(Guid id, int top=10);
}