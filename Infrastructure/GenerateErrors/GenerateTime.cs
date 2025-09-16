using Application;

namespace Infrastructure.GenerateErrors;

public class GenerateTime:IGenerateTime
{
    private readonly Random _random = new();
    private readonly DateOnly _date;

    public GenerateTime(DateOnly date)
    {
        _date = date;
    }
    public DateTimeOffset Generate(DateTime Day)
    {
        int hour = _random.Next(0, 24);
        int minute = _random.Next(0, 60);
        int second = _random.Next(0, 60);
        return _date.ToDateTime(new TimeOnly(hour,minute,second));
    }
}