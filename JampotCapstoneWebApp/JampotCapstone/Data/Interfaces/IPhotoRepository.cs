using File = JampotCapstone.Models.File;

namespace JampotCapstone.Data.Interfaces;

public interface IPhotoRepository
{
    public Task<List<File>> GetAllPhotosAsync();
    public Task<File> GetFileByNameAsync(string name);
    public Task<List<File>> GetFilesByNameAsync(string name);
    public Task<List<File>> GetPhotosByPageAsync(string page);
    public Task<File> GetPhotoByPageAsync(string page);
    public Task<File> GetFileByIdAsync(int id);
    public Task<int> AddFileAsync(File photo);
    public Task<int> UpdateFileAsync(File photo);
    public Task<int> DeleteFileAsync(File photo);
    
}