using File = JampotCapstone.Models.File;

namespace JampotCapstone.Data.Interfaces;

public interface IPhotoRepository
{
    public Task<List<File>> GetAllPhotosAsync();
    public Task<File> GetPhotoByNameAsync(string name);
    public Task<List<File>> GetFilesByNameAsync(string name);
    public Task<List<File>> GetPhotosByPageAsync(string page);
    public Task<File> GetPhotoByPageAsync(string page);
    public Task<File> GetPhotoByIdAsync(int id);
    public Task<int> AddPhotoAsync(File photo);
    public Task<int> UpdatePhotoAsync(File photo);
    public Task<int> DeletePhotoAsync(File photo);
    
}