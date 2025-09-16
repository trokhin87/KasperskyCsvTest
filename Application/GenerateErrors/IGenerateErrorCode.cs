using Domain.Models;

namespace Application;

public interface IGenerateErrorCode
{
    string Generate(string versionString);
}