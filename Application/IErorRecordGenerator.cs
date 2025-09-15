using Domain.Models;

namespace Application;

public interface IErorRecordGenerator
{
    ErrorRecord Generate();
}