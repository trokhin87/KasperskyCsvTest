using Domain.Models;

namespace Application;

public interface IGenerateErrorCode
{
    string Generate(string product, Severity severity);
}