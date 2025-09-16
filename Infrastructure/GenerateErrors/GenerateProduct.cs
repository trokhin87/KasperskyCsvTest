using Application;

namespace Infrastructure.GenerateErrors;

public class GenerateProduct:IGenerateProduct
{
    private readonly WeightedRandom<string> _picker;

    public GenerateProduct()
    {
        var weights = new Dictionary<string, int>()
        {
            {"A", 15},
            {"B", 20},
            {"C", 25},
            {"D", 40}
        };
        _picker=new WeightedRandom<string>(weights);
    }
    public string Generate()
    {
        return _picker.Pick();
    }
}