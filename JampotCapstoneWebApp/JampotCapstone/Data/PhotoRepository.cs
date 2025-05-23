using JampotCapstone.Data.Interfaces;
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
        List<File> photos = await _context.Files
            .Where(p => p.ContentType.Contains("image"))
            .ToListAsync();
        return photos;
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
        List<File> photos = await _context.Files
            .Where(f => f.Pages
                .Any(p => p.PageTitle.ToLower().Contains(page)))
            .ToListAsync();
        return photos;
    }

    public async Task<File> GetPhotoByPageAsync(string page)
    {
        File photo = await _context.Files.Include(f => f.Pages)
            .FirstOrDefaultAsync(f => f.Pages
                .Any(p => p.PageTitle.ToLower().Contains(page.ToLower())));
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