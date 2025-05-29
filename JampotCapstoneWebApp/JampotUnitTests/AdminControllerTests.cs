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
    
    
}