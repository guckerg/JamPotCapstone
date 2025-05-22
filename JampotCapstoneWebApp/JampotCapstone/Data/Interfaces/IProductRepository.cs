using JampotCapstone.Models;

namespace JampotCapstone.Data.Interfaces;

public interface IProductRepository
{
    public Task<List<Product>> GetAllProductsAsync();
    public Task<Product> GetProductByIdAsync(int id);
    public Task<List<Product>> GetProductsByNameAsync(string name);
    public Task<List<Product>> GetProductsByCategoryAsync(string category);
    public Task<List<Product>> GetProductsByTagAsync(string tag);
    public Task<int> AddProductAsync(Product product);
    public Task<int> UpdateProductAsync(Product product);
    public Task<int> DeleteProductAsync(Product product); 
}