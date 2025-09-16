namespace Application;

public interface IGeneratorService
{
    Task<Guid> GenerateAsync(int count = 10_000);
}