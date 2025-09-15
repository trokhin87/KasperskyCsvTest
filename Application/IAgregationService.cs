using Domain.Models;

namespace Application;

public interface IAgregationService
{
    Task<IEnumerable<(Severity severity,int count)>>AggregateBySeverity(Guid Id);
    Task<IEnumerable<(string product,string version,int count)>>AggregateBySeveretyandProduct(Guid Id);
    Task<IEnumerable<(DateTime Hour, string Product, string Version, string ErrorCode, int Count)>> MaxErrorCodePerHourAsync(Guid id);
    Task<IEnumerable<(string ErrorCode, int count)>> AggregateByErrorcode(Guid id, int top=10);
}