using JampotCapstone.Data.Interfaces;
using JampotCapstone.Models;
using File = JampotCapstone.Models.File;

namespace JampotUnitTests;

public class FakeFileRepository : IPhotoRepository
{
    private List<File> _files = new List<File>();

    public async Task<List<File>> GetAllPhotosAsync()
    {
        return _files;
    }

    public async Task<File> GetPhotoByNameAsync(string name)
    {
        return _files.Find(f => f.FileName.ToLower().Contains(name.ToLower()));
    }

    public async Task<List<File>> GetFilesByNameAsync(string name)
    {
        return _files.Where(f => f.FileName.ToLower().Contains(name.ToLower())).ToList();
    }

    public async Task<List<File>> GetPhotosByPageAsync(string page)
    {
        List<File> model = _files
            .Where(f => f.Pages
                .Any(p => p.PageTitle.ToLower()
                    .Contains(page)))
            .ToList();
        return model;
    }

    public async Task<File> GetPhotoByPageAsync(string page)
    {
        File? model = _files
            .Find(f => f.Pages
                .Any(p => p.PageTitle.ToLower()
                    .Contains(page)));
        return model;
    }

    public async Task<File> GetPhotoByIdAsync(int id)
    {
        File? model = _files.Find(t => t.FileID == id);
        return model;
    }

    public async Task<int> AddPhotoAsync(File photo)
    {
        int result = 0;
        if(photo != null && photo.Pages.Count > 0)
        {
            _files.Add(photo);
            _files[_files.Count - 1].FileID = _files.Count;
            result = 1;
        }
        return result;
    }

    public async Task<int> UpdatePhotoAsync(File photo)
    {
        int result = 0;
        int index = photo.FileID - 1;
        if (_files[index] == photo)
        {
            _files[index] = photo;
            result = 1;
        }
        return result;
    }

    public async Task<int> DeletePhotoAsync(File photo)
    {
        int result = 0;
        if (_files[photo.FileID - 1] == photo)
        {
            _files.RemoveAt(photo.FileID - 1);
            result = 1;
        }

        return result;
    }
}