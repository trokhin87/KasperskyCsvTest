using System.Collections.Concurrent;
using Application;
using Domain.Models;

namespace Infrastructure.Stores;

public class StoreService:IStoreService
{
    private readonly ConcurrentDictionary<Guid,List<ErrorRecord>> _storage=new();
    public Guid Save(IEnumerable<ErrorRecord> errorRecords)
    {
        var id = Guid.NewGuid();
        _storage[id] = new List<ErrorRecord>(errorRecords);
        return id;
    }

    public IEnumerable<ErrorRecord> Get(Guid id)
    {
        return _storage[id];
    }

    public void Remove(Guid id)
    {
        _storage.TryRemove(id, out _);
    }
}