namespace Application.DTO;

public record SeverityAggregationDto(string Severity, int Count);
public record ProductVersionAggregationDto(string Product, string Version, int Count);
public record ErrorCodeHourAggregationDto(DateTime Hour, string Product, string Version, string ErrorCode, int Count);
public record ErrorCodeAggregationDto(string ErrorCode, int Count);