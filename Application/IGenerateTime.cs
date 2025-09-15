namespace Application;

public interface IGenerateTime
{
    DateTimeOffset Generate(DateTime Day);
    
}