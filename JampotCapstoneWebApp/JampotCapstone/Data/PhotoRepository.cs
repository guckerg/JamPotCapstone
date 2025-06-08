using JampotCapstone.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = JampotCapstone.Models.File;

namespace JampotCapstone.Data;

public class PhotoRepository : IPhotoRepository
{
    private ApplicationDbContext _context;

    public PhotoRepository(ApplicationDbContext ctx)
    {
        _context = ctx;
    }

    public async Task<List<File>> GetAllPhotosAsync()
    {
        List<File> photos = await _context.Files
            .Where(p => p.ContentType.Contains("image"))
            .Include(f => f.Pages.OrderBy(p => p.Position))
            .ToListAsync();
        return photos;
    }
    public async Task<List<File>> GetPhotosNotInPageAsync(string pageTitle)
    {
        // Get the PageId for the given pageTitle
        var page = await _context.Pages
                                 .FirstOrDefaultAsync(p => p.PageTitle.ToLower() == pageTitle.ToLower());

        if (page == null)
        {
            // If the page doesn't exist, no photos are "on" it, so return all photos.
            return await _context.Files
                                 .Where(p => p.ContentType.Contains("image"))
                                 .ToListAsync();
        }

        //  Get all photos that are images
        var allImagePhotos = _context.Files
                                     .Where(p => p.ContentType.Contains("image"))
                                     .Include(f => f.Pages); // Include Pages for filtering

        // Filter out photos that are linked to the current page
        List<File> photosNotInPage = await allImagePhotos
            .Where(f => !f.Pages.Any(p => p.PageId == page.PageId))
            .ToListAsync();

        return photosNotInPage;
    }

    public async Task<File> GetFileByNameAsync(string name)
    {
        File? photo = await _context.Files.Include(f => f.Pages)
            .FirstOrDefaultAsync(f => f.FileName.ToLower().Contains(name));
        return photo;
    }

    public async Task<List<File>> GetFilesByNameAsync(string name)
    {
        List<File> photos = await _context.Files
            .Where(f => f.FileName.ToLower().Contains(name.ToLower()))
            .ToListAsync();
        return photos;
    }

    public async Task<List<File>> GetPhotosByPageAsync(string page)
    {
        int pageId = _context.Pages.FirstOrDefaultAsync(p => p.PageTitle.ToLower().Contains(page)).Result.PageId;
        List<File> photos = await _context.Files
            .Include(f => f.Pages)
            .Where(f => f.Pages.Any(p => p.PageId == pageId))
            .OrderBy(f => f.Pages.FirstOrDefault(p => p.PageId == pageId).Position)
            .ToListAsync();
        return photos;
    }

    

    public async Task<File> GetPhotoByPageAsync(string page)
    {
        int pageId = _context.Pages.FirstOrDefaultAsync(p => p.PageTitle == page).Result.PageId;
        File? photo = await _context.Files.Include(f => f.Pages)
            .FirstOrDefaultAsync(f => f.Pages.Any(p => p.PageId == pageId));
        return photo;
    }

    public async Task<File> GetFileByIdAsync(int id)
    {
        File photo = await _context.Files.FindAsync(id);
        return photo;
    }

    public async Task<int> AddFileAsync(File photo)
    {
        _context.Files.Add(photo);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateFileAsync(File photo)
    {
        _context.Files.Update(photo);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteFileAsync(File photo)
    {
        _context.Files.Remove(photo);
        return await _context.SaveChangesAsync();
    }
}