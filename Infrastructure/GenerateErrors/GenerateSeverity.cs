using Application;
using Domain.Models;

namespace Infrastructure.GenerateErrors;

public class GenerateSeverity:IGenerateSeverity
{
    private readonly WeightedRandom<Severity> _picker;
    public GenerateSeverity()
    {
        var weights = new Dictionary<Severity, int>
        {
            {Severity.Critical, 5 },
            {Severity.High, 25 },
            {Severity.Normal,40},
            {Severity.Low,20}
        };
        _picker=new WeightedRandom<Severity>(weights);
    }
    public Severity Generate()
    {
        return _picker.Pick();
    }
}