namespace Infrastructure;

public class WeightedRandom<T>
{
    private  readonly Random _random = new();
    private readonly List<(T Item, int CumulativeWeight)> _items = new();
    private int _totalWeight;

    public WeightedRandom(Dictionary<T, int> weights)
    {
        if (weights == null || weights.Count == 0)
            throw new ArgumentException("Weights dictionary cannot be null or empty");

        int cumulative = 0;
        foreach (var kvp in weights)
        {
            cumulative += kvp.Value;
            _items.Add((kvp.Key, cumulative));
        }
        _totalWeight = cumulative;
    }

    public T Pick()
    {
        int value = _random.Next(0, _totalWeight);
        return _items.First(i => value < i.CumulativeWeight).Item;
    }
}