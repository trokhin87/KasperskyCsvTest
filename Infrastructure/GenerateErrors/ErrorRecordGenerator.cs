using Application;
using Domain.Models;

namespace Infrastructure.GenerateErrors;

public class ErrorRecordGenerator:IErorRecordGenerator
{
    private readonly IGenerateTime _timeGenerator;
    private readonly IGenerateSeverity _severityGenerator;
    private readonly IGenerateProduct _productGenerator;
    private readonly IGenerateVersion _versionGenerator;
    private readonly IGenerateErrorCode _errorCodeGenerator;

    public ErrorRecordGenerator(
        IGenerateTime timeGenerator,
        IGenerateSeverity severityGenerator,
        IGenerateProduct productGenerator,
        IGenerateVersion versionGenerator,
        IGenerateErrorCode errorCodeGenerator)
    {
        _timeGenerator = timeGenerator;
        _severityGenerator = severityGenerator;
        _productGenerator = productGenerator;
        _versionGenerator = versionGenerator;
        _errorCodeGenerator = errorCodeGenerator;
    }

    public ErrorRecord Generate()
    {
        // Генерируем timestamp
        var timestamp = _timeGenerator.Generate(DateTime.Now);

        // Генерируем severity
        var severity = _severityGenerator.Generate();

        // Генерируем продукт + версии
        var versionString = _versionGenerator.Generate(); // например "A1.1 B2.5 C1.0 D2.3"

        // Генерируем error code для каждой версии
        var errorCodeString = _errorCodeGenerator.Generate(versionString); 
        // например "A1.1-ERR42 B2.5-ERR7 C1.0-ERR15 D2.3-ERR99"

        return new ErrorRecord
        {
            Timestamp = timestamp,
            Severity = severity,
            Version = versionString,
            ErrorCode = errorCodeString
        };
    }
}