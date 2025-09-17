using System.Text;
using Application;

namespace Infrastructure;

public class GenerateVersion:IGenerateVersion
{
    private readonly WeightedRandom<string> _picker;
    private readonly IGenerateProduct _generatedProducts;
    private readonly List<string> _versions;
    public GenerateVersion(IGenerateProduct productGenerator)
    {
        _generatedProducts = productGenerator;

        #region DictWeight
        var weights = new Dictionary<string, int>()
        {
            { "1.0", 4 },
            { "1.1", 3 },
            { "1.2", 2 },
            { "1.3", 1 },
            { "1.4", 6 },
            { "1.5", 7 },
            { "1.6", 8 },
            { "1.7", 9 },
            { "1.8", 1 },
            { "1.9", 9 },
            { "2.0", 2 },
            { "2.1", 8 },
            { "2.2", 3 },
            { "2.3", 7 },
            { "2.4", 4 },
            { "2.5", 6 },
            { "2.6", 5 },
            { "2.7", 5 },
            { "2.8", 5 },
            { "2.9", 5 },
        };
        #endregion
        _picker=new WeightedRandom<string>(weights);
        _versions=weights.Keys.ToList();

    }

    public string Generate(string product)
    {
        var sb = new StringBuilder();
            var version = _picker.Pick();
            sb.Append(product);
            sb.Append(version);
            sb.Append(" ");

        return sb.ToString().Trim();
    }
}