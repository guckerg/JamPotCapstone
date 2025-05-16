using Microsoft.EntityFrameworkCore;
using File = JampotCapstone.Models.File;
using JampotCapstone.Models;

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
        List<File> photos = await _context.Files.Where(p => p.ContentType.Contains("image")).ToListAsync();
        return photos;
    }

    public async Task<File> GetPhotoByNameAsync(string name)
    {
        File photo = await _context.Files
            .FirstOrDefaultAsync(p => p.FileName.ToLower().Contains(name));
        return photo;
    }

    public async Task<List<File>> GetPhotosByPageAsync(string page)
    {
        List<File> photos = [];
        Page currentPage = await _context.Pages.Where(p => p.PageTitle.ToLower().Contains(page.ToLower()))
            .Include(p => p.Files)
            .FirstOrDefaultAsync();
        if (currentPage != null)
        {
            photos = currentPage.Files;
        }

        return photos;
    }

    public async Task<File> GetPhotoByPageAsync(string page)
    {
        File photo = await _context.Files
            .FirstOrDefaultAsync(f => f.Pages
                .Any(p => p.PageTitle.ToLower().Contains(page.ToLower())));
        return photo;
    }

    public async Task<File> GetPhotoByIdAsync(int id)
    {
        File photo = await _context.Files.FindAsync(id);
        return photo;
    }

    public async Task<int> AddPhotoAsync(File photo)
    {
        _context.Files.Add(photo);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdatePhotoAsync(File photo)
    {
        _context.Files.Update(photo);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeletePhotoAsync(File photo)
    {
        _context.Files.Remove(photo);
        return await _context.SaveChangesAsync();
    }
}