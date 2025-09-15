using Domain.Models;

namespace Application;

public interface IStoreService
{
    Guid Save(IEnumerable<ErrorRecord> errorRecords);
    IEnumerable<ErrorRecord> Get(Guid id);
    void Remove(Guid id);
    
}