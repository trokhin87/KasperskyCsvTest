using Domain.Models;

namespace Application;

public interface IAgregationService
{
    Task<IEnumerable<(Severity severity,int count)>>AggregateBySeverity(Guid Id);
}