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
        // Create ProductPhoto instances
        var photo1 = new File { FileID = 1, FileName = "Juice.png" };
        var photo2 = new File { FileID = 2, FileName = "Wrap.png" };

        // Create Product instances and associate them
        var product1 = new Product
        {
            ProductId = 1,
            ProductName = "Classic Juice",
            ProductPrice = 10.00m,
            ProductIngredients = "Apple juice, grape, pineapple",
            ProductPhoto = photo1, 
            ProductCategory = null, 
            Tags = null 
        };
        // Make product tags and type null until used in a test. 
        // This is because if they're created in the constructor, an infinite loop is created.

        var product2 = new Product
        {
            ProductId = 2,
            ProductName = "Tasty Wrap",
            ProductPrice = 4.00m,
            ProductIngredients = "Tortilla, salt, cabbage",
            ProductPhoto = photo2,
            ProductCategory = null,
            Tags = null
        };

        // Add products to the context
        context.Products.AddRange(product1, product2);
        context.SaveChanges();
    }

    [Fact]
    public void ReturnItemsToView_Success()
    {
        // Arrange
        var context = CreateInMemoryDbContext();
        SeedProducts(context);

        var mockSession = new TestSession(); //Simulate sessions

        var product1 = context.Products.Find(1);
        var product2 = context.Products.Find(2);
        // Create tags and types
        var glutenFreeTag = new ProductTag { TagID = 2, Tag = "gluten free" };
        var veganTag = new ProductTag { TagID = 1, Tag = "vegan" };
        var foodType = new ProductType { TypeId = 1, Type = "food" };
        var drinkType = new ProductType { TypeId = 2, Type = "drink" };


        // Avoid full navigation properties to prevent endless cycles
        product1.ProductCategory = new List<ProductType> { drinkType };
        product1.Tags = new List<ProductTag> { glutenFreeTag };
        product2.ProductCategory = new List<ProductType> { foodType };
        product2.Tags = new List<ProductTag> { glutenFreeTag, veganTag };

        var cartItems = new List<OrderItem>
        {
            new OrderItem { ProductId = 1, Quantity = 1, Product = product1 },
            new OrderItem { ProductId = 2, Quantity = 2, Product = product2 }
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
        // Quantity of different products
        Assert.Equal(2, viewModel.Items.Count);
        // Make sure name is the right name
        Assert.Equal("Classic Juice", viewModel.Items[0].ProductName);
        Assert.Equal("Tasty Wrap", viewModel.Items[1].ProductName);
    }

    [Fact]
    public void AddToCart_Success()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        SeedProducts(context);

        var mockSession = new TestSession();
        var mockHttpContext = new DefaultHttpContext { Session = mockSession };
        var controllerContext = new ControllerContext { HttpContext = mockHttpContext };

        var controller = new CartController(context)
        {
            ControllerContext = controllerContext
        };

        var product2 = context.Products.Find(2);
        // Create tags and types
        var foodType = new ProductType { TypeId = 1, Type = "food" };
        var glutenFreeTag = new ProductTag { TagID = 2, Tag = "gluten free" };
        product2.ProductCategory = new List<ProductType> { foodType };
        product2.Tags = new List<ProductTag> { glutenFreeTag };

        var request = new CartController.AddToCartRequest { ProductId = 2 };

        // Act
        var result = controller.AddToCart(request) as JsonResult;
        var cartItems = mockSession.GetObjectFromJson<List<OrderItem>>("CartItems");

        // Assert
        Assert.NotNull(result);
        // Make sure the sessions contains the added productId
        Assert.Contains(cartItems, i => i.ProductId == 2);
        // Check for correct quantity
        Assert.Contains(cartItems, i => i.Quantity == 1);
        // Check correct product price
        Assert.Contains(cartItems, i => i.Product.ProductPrice == 4.00m);
    }

    [Fact]
    public void AddToCart_Failure()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        SeedProducts(context); // Only product 1 and 2 exist

        var mockSession = new TestSession();
        var mockHttpContext = new DefaultHttpContext { Session = mockSession };
        var controllerContext = new ControllerContext { HttpContext = mockHttpContext };

        var controller = new CartController(context)
        {
            ControllerContext = controllerContext
        };

        var request = new CartController.AddToCartRequest { ProductId = 999 }; // Non-existent product

        // Act
        var result = controller.AddToCart(request);

        // Assert
        // Checks if the product wasn't found
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void UpdateCartAdd_Success()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        SeedProducts(context);

        var mockSession = new TestSession();

        var product1 = context.Products.Find(1);
        // Create tags and types
        var drinkType = new ProductType { TypeId = 2, Type = "drink" };
        var glutenFreeTag = new ProductTag { TagID = 2, Tag = "gluten free" };
        product1.ProductCategory = new List<ProductType> { drinkType };
        product1.Tags = new List<ProductTag> { glutenFreeTag };

        var existingCartItems = new List<OrderItem>
        {
            new OrderItem { ProductId = 1, Quantity = 1, Product = product1}
        };
        mockSession.SetObjectAsJson(_cartSessionKey, existingCartItems);

        var mockHttpContext = new DefaultHttpContext { Session = mockSession };
        var controllerContext = new ControllerContext { HttpContext = mockHttpContext };

        var controller = new CartController(context)
        {
            ControllerContext = controllerContext
        };

        var request = new CartController.AddToCartRequest { ProductId = 1 };

        // Act
        var result = controller.AddToCart(request) as JsonResult;
        var cartItems = mockSession.GetObjectFromJson<List<OrderItem>>("CartItems");

        // Assert
        Assert.NotNull(result);
        // Make sure the sessions contains the added productId
        Assert.Contains(cartItems, i => i.ProductId == 1);
        // Check for correct quantity
        Assert.Contains(cartItems, i => i.Quantity == 2);
    }

    [Fact]
    public void UpdateCartMinus_Success()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        SeedProducts(context);

        var mockSession = new TestSession();

        var product1 = context.Products.Find(1);
        // Create tags and types
        var drinkType = new ProductType { TypeId = 2, Type = "drink" };
        var glutenFreeTag = new ProductTag { TagID = 2, Tag = "gluten free" };
        product1.ProductCategory = new List<ProductType> { drinkType };
        product1.Tags = new List<ProductTag> { glutenFreeTag };

        var existingCartItems = new List<OrderItem>
        {
            new OrderItem { ProductId = 1, Quantity = 1, Product = product1}
        };
        mockSession.SetObjectAsJson(_cartSessionKey, existingCartItems);

        var mockHttpContext = new DefaultHttpContext { Session = mockSession };
        var controllerContext = new ControllerContext { HttpContext = mockHttpContext };

        var controller = new CartController(context)
        {
            ControllerContext = controllerContext
        };

        var request = new CartController.RemoveFromCartRequest { ProductId = 1 };

        // Act
        var result = controller.RemoveFromCart(request) as JsonResult;
        var cartItems = mockSession.GetObjectFromJson<List<OrderItem>>("CartItems");

        // Assert
        Assert.NotNull(result);
        // Check for quantity less than 1
        Assert.DoesNotContain(cartItems, i => i.Quantity <= 1);
    }

    [Fact]
    public void UpdateCart_Failure()
    // trying to remove a product that is not in the cart
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        SeedProducts(context);

        var mockSession = new TestSession();

        var product2 = context.Products.Find(2);
        // Create tags and types
        var foodType = new ProductType { TypeId = 2, Type = "food" };
        var glutenFreeTag = new ProductTag { TagID = 2, Tag = "gluten free" };
        product2.ProductCategory = new List<ProductType> { foodType };
        product2.Tags = new List<ProductTag> { glutenFreeTag };

        var existingCartItems = new List<OrderItem>
        {
            new OrderItem { ProductId = 2, Quantity = 1, Product = product2}
        };
        mockSession.SetObjectAsJson(_cartSessionKey, existingCartItems);

        var mockHttpContext = new DefaultHttpContext { Session = mockSession };
        var controllerContext = new ControllerContext { HttpContext = mockHttpContext };

        var controller = new CartController(context)
        {
            ControllerContext = controllerContext
        };

        var request = new CartController.RemoveFromCartRequest { ProductId = 1 }; // Item not in cart

        // Act
        var result = controller.RemoveFromCart(request) as JsonResult;
        var cartItems = mockSession.GetObjectFromJson<List<OrderItem>>("CartItems");

        // Assert
        Assert.NotNull(result);
        // Make sure cart still contains product 2
        Assert.Contains(cartItems, i => i.ProductId == 2);
        // Check for correct quantity
        Assert.Contains(cartItems, i => i.Quantity == 1);
    }

    [Fact]
    public void GetCartQuantity_EmptyCart()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        // Don't seed product so it defaults to an empty list 
        var mockSession = new TestSession();
        var mockHttpContext = new DefaultHttpContext { Session = mockSession };
        var controllerContext = new ControllerContext { HttpContext = mockHttpContext };

        var controller = new CartController(context)
        {
            ControllerContext = controllerContext
        };

        // Act
        var result = controller.GetCartQuantity() as JsonResult;
        var cartItems = mockSession.GetObjectFromJson<List<OrderItem>>("CartItems") ?? new List<OrderItem>();

        // Assert
        Assert.NotNull(result);
        // Check for correct quantity
        Assert.Empty(cartItems);
    }
}