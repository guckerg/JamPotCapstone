using JampotCapstone.Data.Interfaces;
using JampotCapstone.Models;
using Microsoft.EntityFrameworkCore;

namespace JampotUnitTests;

public class FakeProductRepository : IProductRepository
{
    private readonly List<Product> _products = [];

    public async Task<List<ProductTag>> GetAllProductTagsAsync()
    {
        // Flatten all tags from all products, select distinct tags, and convert to list
        List<ProductTag> tags = _products
                                .SelectMany(p => p.Tags)
                                .GroupBy(t => t.Tag) // Group by tag string to get distinct tags
                                .Select(g => g.First()) // Select the first tag from each group
                                .ToList();
        return await Task.FromResult(tags); // Wrap in Task.FromResult for async signature
        // Return a list of all product tags for drop down list
    }
    public async Task<List<ProductType>> GetAllProductTypesAsync()
    {
        // Flatten all product types from all products, select distinct types, and convert to list
        List<ProductType> types = _products
                                  .SelectMany(p => p.ProductCategory)
                                  .GroupBy(pt => pt.Type) // Group by type string to get distinct types
                                  .Select(g => g.First()) // Select the first type from each group
                                  .ToList();
        return await Task.FromResult(types); // Wrap in Task.FromResult for async signature
        // Return a list of all product categories for drop down list
    }

    public Task<List<ProductTag>> GetTagsByIdsAsync(List<int> tagIds)
    {
        throw new NotImplementedException();
    }
    public Task<ProductType> GetProductTypeByIdAsync(int typeId)
    {
        throw new NotImplementedException();
    }
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