using Application;

namespace Infrastructure.GenerateErrors;

public class GenerateProduct:IGenerateProduct
{
    private readonly WeightedRandom<string> _picker;
    private readonly List<string> _products;
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
        _products=weights.Keys.ToList();
    }
    public string Generate()
    {
        return _picker.Pick();
    }

    public IReadOnlyCollection<string> GetAllProducts()=> _products;
    
}