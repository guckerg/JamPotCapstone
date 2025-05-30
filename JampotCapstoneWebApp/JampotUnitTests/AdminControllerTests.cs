using Microsoft.AspNetCore.Mvc;

namespace JampotUnitTests;

public class AdminControllerTests
{
    private AdminController _admin;
    private IPhotoRepository _photoRepo;
    private IPageRepository _pageRepo;
    private IProductRepository _prodRepo;
    private IPagePositionRepository _posRepo;
    private ITextElementRepository _textRepo;

    public AdminControllerTests()
    {
        _photoRepo = new FakeFileRepository();
        _pageRepo = new FakePageRepository();
        _prodRepo = new FakeProductRepository();
        _posRepo = new FakePagePositionRepository();
        _textRepo = new FakeTextElementRepository();
        _admin = new AdminController(_textRepo, _photoRepo, _pageRepo, _prodRepo, _posRepo);
    }

    [Fact]
    public void TestTextEdit_Success()
    {
        TextElement model = new TextElement
        {
            TextElementId = 1,
            Name = "Sample Text",
            Content = "Sample Content",
            PageId = 1,
            Page = new Page()
        };
        var result = _admin.TextEdit(model);
        Assert.IsType<Task<RedirectToActionResult>>(result);
    }
}