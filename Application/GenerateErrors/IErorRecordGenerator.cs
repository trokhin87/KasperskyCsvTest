using Domain.Models;

namespace Application;

public interface IErorRecordGenerator
{
    IEnumerable<ErrorRecord> Generate();
}