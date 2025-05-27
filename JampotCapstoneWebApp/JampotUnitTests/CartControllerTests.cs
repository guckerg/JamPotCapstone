using JampotCapstone.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JampotUnitTests;

public class CartControllerTests
{
    private readonly string _cartSessionKey = "CartItems";

    // Method to set up an in-memory DbContext
    // in-memory is for sessions
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
        // Arrange
        var context = CreateInMemoryDbContext();
        SeedProducts(context);

        var mockSession = new TestSession(); //Simulate sessions
        var cartItems = new List<OrderItem> 
        {
            new OrderItem { ProductId = 1, Quantity = 2, Product = context.Products.Find(1) },
            new OrderItem { ProductId = 2, Quantity = 1, }
        };
        mockSession.SetObjectAsJson(_cartSessionKey, cartItems);

        var mockHttpContext = new DefaultHttpContext { Session = mockSession };
        var controllerContext = new ControllerContext { HttpContext = mockHttpContext };

        var controller = new CartController(context)
        {
            ControllerContext = controllerContext
        };

        // Act
        var result = controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        // make sure viewModel gets populated
        var viewModel = result.Model as CartViewModel;
        Assert.NotNull(viewModel);
    }

    [Fact]
    public void AddToCart_Failure()
    {
        // Arrange
        // Act
        // Assert
    }

    [Fact]
    public void UpdateCart_Success()
    {
        // Arrange
        // Act
        // Assert
    }

    [Fact]
    public void UpdateCart_Faliure()
    {
        // Arrange
        // Act
        // Assert
    }

    [Fact]
    public void GetCartQuantity_Success()
    {
        // Arrange
        // Act
        // Assert
    }
    [Fact]
    public void GetCartQuantity_Failure()
    {
        // Arrange
        // Act
        // Assert
    }
}