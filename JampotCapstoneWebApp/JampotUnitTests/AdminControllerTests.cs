using System.Text;
using JampotCapstone.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Moq;
using Xunit.Abstractions;

namespace JampotUnitTests;

public class AdminControllerTests
{
    private AdminController _admin;
    private IPhotoRepository _photoRepo;
    private IPageRepository _pageRepo;
    private IProductRepository _prodRepo;
    private IPagePositionRepository _posRepo;
    private ITextElementRepository _textRepo;
    private ITestOutputHelper _output;

    public AdminControllerTests(ITestOutputHelper output)
    {
        _output = output;
        _photoRepo = new FakeFileRepository();
        _pageRepo = new FakePageRepository();
        _prodRepo = new FakeProductRepository();
        _posRepo = new FakePagePositionRepository();
        _textRepo = new FakeTextElementRepository();
        _admin = new AdminController(_textRepo, _photoRepo, _pageRepo, _prodRepo, null, _posRepo, null, null);
        _textRepo.StoreTextElementAsync(
            new TextElement
            {
                TextElementId = 1,
                Name = "Sample Text",
                Content = "Sample Content",
                PageId = 1,
                Page = new Page()
            });
        _pageRepo.StorePageAsync(new Page
        {
            PageId = 1,
            Files = [],
            PageTitle = "FAQ"
        });
        _pageRepo.StorePageAsync(new Page
        {
            PageId = 2,
            Files = [],
            PageTitle = "Home"
        });
        _posRepo.StorePagePosition(new PagePosition
        {
            PageId = 2,
            FileId = 1,
            Position = 1
        });
        
        _prodRepo.AddProductAsync(new Product
            {
                ProductId = 1,
                ProductName = "Garden Wrap",
                ProductIngredients = "Spinach Tortilla, Lettuce, Carrot, Purple Cabbage, Tomato, Cucumber, Bell Pepper, Alfalfa Sprouts, Roasted Red Pepper Hummus",
                ProductPrice = 14,
                ProductPhoto = new File(),
                ProductCategory = new List<ProductType>()
                {
                    new ProductType
                    {
                        TypeId = 1,
                        Type = "food"
                    }
                },
                Tags = new List<ProductTag>()
                {
                    new ProductTag
                    {
                        TagID = 1,
                        Tag = "vegan"
                    }
                }
            });
            
            _prodRepo.AddProductAsync(new Product
            {
                ProductId = 2,
                ProductName = "Jerk Chicken Wrap",
                ProductIngredients = "Spinach Tortilla, Lettuce, Carrot, Purple Cabbage, Bell Pepper, Alfalfa Sprouts, Jerk Sauce",
                ProductPrice = 15,
                ProductPhoto = new File(),
                ProductCategory = new List<ProductType>()
                {
                    _prodRepo.GetProductByIdAsync(1).Result.ProductCategory.First(),
                },
                Tags = new List<ProductTag>()
                {
                    new ProductTag
                    {
                        TagID = 2,
                        Tag = "spicy"
                    }
                }
            });
            
            _prodRepo.AddProductAsync(new Product
            {
                ProductId = 3,
                ProductName = "Reggae Chia Pudding",
                ProductIngredients = "Coconut milk chia pudding with Hawaiian spirulina, topped with fresh strawberry, mango, and hemp hearts.",
                ProductPrice = 7.50M,
                ProductPhoto = new File(),
                ProductCategory = new List<ProductType>()
                {
                    new ProductType
                    {
                        TypeId = 2,
                        Type = "dessert"
                    }
                },
                Tags = new List<ProductTag>()
                {
                    _prodRepo.GetProductByIdAsync(1).Result.Tags.FirstOrDefault(t => t.TagID == 1),
                    new ProductTag
                    {
                        TagID = 2,
                        Tag = "gluten free"
                    }
                }
            });
    }

    [Fact]
    public void TestTextEdit_Success()
    {
        var model = new TextElement
        {
            TextElementId = 0,
            Name = "Sample Text",
            Content = "Sample Content",
            PageId = 1
        };
        var result = _admin.TextEdit(model).Result;
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public void TestTextEdit_Failure()
    {
        var model = new TextElement
        {
            Name = "Second Sample Text",
            Content = "Second Sample Content",
            TextElementId = 1,
            PageId = 2
        };
        var result = _admin.TextEdit(model).Result;
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void TestEditPhoto_Success()
    {
        EditViewModel model = new EditViewModel
        {
            CurrentPage = "Home",
            Photos =
            [
                new File
                {
                    FileID = 1,
                    FileName = "Sample Image",
                    ContentType = "image/jpeg"
                },
                new File
                {
                    FileID = 2,
                    FileName = "Sample Image 2",
                    ContentType = "image/jpeg"
                }
            ],
            NewPhotoId = 2,
            OldPhotoId = 1,
        };
        var result = _admin.EditPhoto(model).Result;
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public void TestEditPhoto_Failure()
    {
        EditViewModel model = new EditViewModel
        {
            CurrentPage = "FAQ",
            NewPhotoId = 2,
            OldPhotoId = 1,
            Photos = []
        };
        var result = _admin.EditPhoto(model).Result;
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void TestAddPhoto_Success()
    {
        // Creating a mock IFormFile for the addPhoto method parameter
        var model = GetMockFile("test.jpg", "image/jpeg");

        // Act
        var result = _admin.AddPhoto(model).Result;
        var files = _photoRepo.GetAllPhotosAsync().Result;
        var newPhoto = files.Find(f => f.FileName.Contains(model.FileName));
        Assert.IsType<RedirectToActionResult>(result);
        // since a RedirectToActionResult could mean either success or failure, check that new file was added to the repo
        Assert.NotNull(newPhoto);
    }
    
    [Fact]
    public void TestAddPhoto_InvalidType()
    {
        var model = GetMockFile("test.pdf", "application/pdf");
        var result = _admin.AddPhoto(model).Result;
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void TestProductEditGet_Success()
    {
        var result = _admin.ProductEdit(2).Result;
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void TestProductEditGet_Failure()
    {
        var result = _admin.ProductEdit(4).Result;
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public void TestProductEditPost_Success()
    {
        var prod = _prodRepo.GetProductByIdAsync(1).Result;
        ProductEditViewModel model = new ProductEditViewModel
        {
            ProductId = 1,
            ProductName = prod.ProductName,
            ProductIngredients = prod.ProductIngredients,
            ProductPrice = prod.ProductPrice,
            SelectedTagIds = [],
            SelectedTypeId = prod.ProductCategory.First().TypeId,
            Tags = [],
            Types = null,
            PhotoUpload = null
        };

        var result = _admin.ProductEdit(prod.ProductId, model).Result;
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public void TestProductEditPost_Failure()
    {
        var prod = _prodRepo.GetProductByIdAsync(1).Result;
        ProductEditViewModel model = new ProductEditViewModel
        {
            ProductId = 1,
            ProductName = prod.ProductName,
            ProductIngredients = prod.ProductIngredients,
            ProductPrice = prod.ProductPrice,
            SelectedTagIds = [],
            SelectedTypeId = 0,
            Tags = []
        };
        var result = _admin.ProductEdit(prod.ProductId, model).Result;
        Assert.IsType<ViewResult>(result);
    }

    public IFormFile? GetMockFile(string fileName, string contentType)
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        var content = "Fake image data";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        mockFile.Setup(_ => _.OpenReadStream()).Returns(stream);
        mockFile.Setup(_ => _.FileName).Returns(fileName);
        mockFile.Setup(_ => _.Length).Returns(stream.Length);
        mockFile.Setup(_ => _.ContentType).Returns(contentType);
        return mockFile.Object;
    }
}