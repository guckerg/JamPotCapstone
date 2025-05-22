using JampotCapstone.Data.Interfaces;
using JampotCapstone.Models;
using Microsoft.EntityFrameworkCore;

namespace JampotCapstone.Data;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext ctx)
    {
        _context = ctx;
    }
    
    public async Task<List<ProductTag>> GetAllProductTagsAsync()
    {
        List<ProductTag> tags = await _context.ProductTags.ToListAsync();
        return tags;
        // Return a list of all product tags for drop down list
    }
    public async Task<List<ProductType>> GetAllProductTypesAsync()
    {
        List<ProductType> types = await _context.ProductTypes.ToListAsync();
        return types;
        // Return a list of all product categories for drop down list
    }
    public async Task<List<Product>> GetAllProductsAsync()
    {
        List<Product> products = await _context.Products // get a list of products
            .Include(p => p.ProductCategory) // including the categories to which they belong
            .Include(p => p.Tags) // and the tags they are marked with
            .Include(p => p.ProductPhoto)
            .ToListAsync();
        return products;
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        Product? product = await _context.Products
            .Include(p => p.ProductCategory)
            .Include(p => p.Tags)
            .Include(p => p.ProductPhoto)
            .FirstOrDefaultAsync(p => p.ProductId == id);
        return product;     // remember to check for nulls where the method is invoked
    }

    public async Task<List<Product>> GetProductsByNameAsync(string name)
    {
        List<Product> products  = await _context.Products
            .Where(p => p.ProductName.ToLower().Contains(name))
            .Include(p => p.ProductPhoto)
            .Include(p => p.ProductCategory)
            .Include(p => p.Tags)
            .ToListAsync();
        return products;
    }

    public async Task<List<Product>> GetProductsByCategoryAsync(string category)
    {
        List<Product> products = await _context.Products
            .Where(p => p.ProductCategory
                .Any(c => c.Type.ToLower()
                    .Contains(category.ToLower())))
            .Include(p => p.ProductPhoto)
            .Include(p => p.ProductCategory)
            .Include(p => p.Tags)
            .ToListAsync();
        return products;
    }

    public async Task<List<Product>> GetProductsByTagAsync(string tag)
    {
        List<Product> products = await _context.Products
            .Where(p => p.Tags
                .Any(t => t.Tag.ToLower()
                    .Contains(tag.ToLower())))
            .Include(p => p.ProductPhoto)
            .Include(p => p.ProductCategory)
            .Include(p => p.Tags)
            .ToListAsync();
        return products;
    }

    public async Task<int> AddProductAsync(Product product)
    {
        _context.Products.Add(product);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateProductAsync(Product product)
    {
        _context.Products.Update(product);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteProductAsync(Product product)
    {
        _context.Products.Remove(product);
        return await _context.SaveChangesAsync();
    }
}