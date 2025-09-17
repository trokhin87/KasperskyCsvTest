using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Models;

namespace Infrastructure;

public class CsvFileService
{
    private readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "Data");

    public CsvFileService()
    {
        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);
    }

    public Guid Save(IEnumerable<ErrorRecord> records,Guid id)
    {
        var filePath = Path.Combine(_basePath, $"{id}.csv");

        using var writer = new StreamWriter(filePath);
        using var csv = new CsvHelper.CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true
        });

        // Пишем заголовки
        csv.WriteField("Timestamp");
        csv.WriteField("Severity");
        csv.WriteField("Product");
        csv.WriteField("Version");
        csv.WriteField("ErrorCode");
        csv.NextRecord();

        foreach (var record in records)
        {
            var versions = record.Version.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var codes = record.ErrorCode.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < versions.Length; i++)
            {
                var version = versions[i];
                var product = version.Substring(0, 1); // A/B/C/D
                var errorCode = codes.FirstOrDefault(c => c.StartsWith(version));

                csv.WriteField(record.Timestamp);
                csv.WriteField(record.Severity);
                csv.WriteField(product);
                csv.WriteField(version);
                csv.WriteField(errorCode);
                csv.NextRecord();
            }
        }

        return id;
    }
    public IEnumerable<ErrorRecord> Read(Guid id)
    {
        var filePath = Path.Combine(_basePath, $"{id}.csv");
        if (!File.Exists(filePath)) return Enumerable.Empty<ErrorRecord>();

        using var reader = new StreamReader(filePath, Encoding.UTF8);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<ErrorRecord>().ToList();
    }
    public async Task SaveToCsvAsync<T>(IEnumerable<T> records, string filePath)
    {
        await using var writer = new StreamWriter(filePath);
        using var csv = new CsvHelper.CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
        await csv.WriteRecordsAsync(records);
    }
}