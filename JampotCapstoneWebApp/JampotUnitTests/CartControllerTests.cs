namespace JampotUnitTests;

public class CartControllerTests
{
    private readonly string _cartSessionKey = "CartItems";

    // Method to set up an in-memory DbContext
    private ApplicationDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
            .Options;
        var context = new ApplicationDbContext(options);
        return context;
    }

    // Helper method to seed data for products
    private void SeedProducts(ApplicationDbContext context)
    {
        // 1. Create ProductPhoto instances
        var photo1 = new File { FileID = 1, FileName = "burger.jpg" };
        var photo2 = new File { FileID = 2, FileName = "fries.jpg" };

        // 2. Create ProductType instances
        var foodType = new ProductType { TypeId = 1, Type = "food" };
        var drinkType = new ProductType { TypeId = 2, Type = "drink" };

        // 3. Create ProductTag instances
        var veganTag = new ProductTag { TagID = 1, Tag = "vegan" };
        var glutenFreeTag = new ProductTag { TagID = 2, Tag = "gluten free" };

        // Add related entities to their respective DbSets first
        context.Files.AddRange(photo1, photo2);
        context.ProductTypes.AddRange(foodType, drinkType);
        context.ProductTags.AddRange(veganTag, glutenFreeTag);
        context.SaveChanges(); // Save changes to assign Ids

        // 4. Create Product instances and associate them
        var product1 = new Product
        {
            ProductId = 1,
            ProductName = "Classic Burger",
            ProductPrice = 12.50m,
            ProductIngredients = "Beef patty, lettuce, tomato, cheese",
            ProductPhoto = photo1, 
            ProductCategory = new List<ProductType> { foodType }, 
            Tags = new List<ProductTag> { glutenFreeTag } 
        };

        var product2 = new Product
        {
            ProductId = 2,
            ProductName = "Crispy Fries",
            ProductPrice = 4.00m,
            ProductIngredients = "Potatoes, salt",
            ProductPhoto = photo2,
            ProductCategory = new List<ProductType> { foodType },
            Tags = new List<ProductTag> { veganTag, glutenFreeTag }
        };

        // Add products to the context
        context.Products.AddRange(product1, product2);
        context.SaveChanges();
    }

    [Fact]
    public void AddToCart_Success()
    {
        // Arange
        // Act
        // Assert
    }

    [Fact]
    public void AddToCart_Failure()
    {
        // Arange
        // Act
        // Assert
    }
}