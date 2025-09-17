using Application.DTO;
using Domain.Models;

namespace Application;

public interface IAgregationService
{
    Task<IEnumerable<SeverityAggregationDto>>AggregateBySeverityAsync(Guid Id);
    Task<IEnumerable<ProductVersionAggregationDto>>AggregateByProductandVersionAsync(Guid Id);
    Task<IEnumerable<ErrorCodeHourAggregationDto>> MaxErrorCodePerHourAsync(Guid id);
    Task<IEnumerable<ErrorCodeAggregationDto>> AggregateByErrorcodeAsync(Guid id, int top=10);
    Task<string> SaveAggregationToCsvAsync<T>(IEnumerable<T> aggregation, string fileName);
}