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

    public IEnumerable<ErrorRecord> Generate()
    {
        var timestamp = _timeGenerator.Generate(DateTime.Now);
        var severity = _severityGenerator.Generate();
        var products = _productGenerator.GetAllProducts().ToList();

        var records = new List<ErrorRecord>();
        foreach (var product in products)
        {
            var version = _versionGenerator.Generate(product); // версия для конкретного продукта
            var errorCode = _errorCodeGenerator.Generate(version); // error для этой версии
            records.Add(new ErrorRecord
            {
                Timestamp = timestamp,
                Severity = severity,
                Product = product,
                Version = version,
                ErrorCode = errorCode
            });
        }

        return records;
    }
}