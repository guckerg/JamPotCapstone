using JampotCapstone.Data.Interfaces;
using JampotCapstone.Models;

namespace JampotUnitTests;

public class FakeProductRepository : IProductRepository
{
    private readonly List<Product> _products = [];
    
    public async Task<List<Product>> GetAllProductsAsync()
    {
        return _products;
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        Product? product = _products.Find(p => p.ProductId == id);
        return product;     // remember to check for nulls where the method is invoked
    }

    public async Task<List<Product>> GetProductsByNameAsync(string name)
    {
        List<Product> products = _products
            .Where(p => p.ProductName.ToLower().Contains(name.ToLower()))
            .ToList();
        return products;
    }

    public async Task<List<Product>> GetProductsByCategoryAsync(string category)
    {
        List<Product> products = _products
            .Where(p => p.ProductCategory
                .Any(c => c.Type.ToLower()
                    .Contains(category.ToLower())))
            .ToList();
        return products;
    }

    public async Task<List<Product>> GetProductsByTagAsync(string tag)
    {
        List<Product> products = _products
            .Where(p => p.Tags
                .Any(t => t.Tag.ToLower()
                    .Contains(tag.ToLower())))
            .ToList();
        return products;
    }

    public async Task<int> AddProductAsync(Product product)
    {
        int result = 0;
        if(product != null && product.ProductPhoto != null)
        {
            _products.Add(product);
            _products[_products.Count - 1].ProductId = _products.Count;
            result = 1;
        }
        return result;
    }

    public async Task<int> UpdateProductAsync(Product product)
    {
        int result = 0;
        int index = product.ProductId - 1;
        if (_products[index] == product)
        {
            _products[index] = product;
            result = 1;
        }
        return result;
    }

    public async Task<int> DeleteProductAsync(Product product)
    {
        int result = 0;
        if (_products[product.ProductId - 1] == product)
        {
            _products.RemoveAt(product.ProductId - 1);
            result = 1;
        }

        return result;
    }
}