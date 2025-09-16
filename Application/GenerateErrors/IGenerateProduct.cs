namespace Application;

public interface IGenerateProduct
{
    string Generate();
    IReadOnlyCollection<string> GetAllProducts();

}