using Domain.Models;

namespace Application;

public interface IGenerateSeverity
{
    Severity Generate();
}