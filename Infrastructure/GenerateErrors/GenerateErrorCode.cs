using System.Text;
using Application;
using Domain.Models;

namespace Infrastructure.GenerateErrors;

public class GenerateErrorCode:IGenerateErrorCode
{
    private readonly Random _random = new();

    public string Generate(string versionString)
    {
        var versions = versionString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var sb = new StringBuilder();

        foreach (var version in versions)
        {
            int codeNumber = _random.Next(1, 101); // 1..100
            sb.Append($"{version}-ERR{codeNumber} ");
        }

        return sb.ToString().Trim();
    }

}