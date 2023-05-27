namespace Blog.Data.FileManager;

public interface IFileManager
{
    FileStream? GetFileStream(string fileName);
    Task<string> SaveFile(IFormFile file);
    bool RemoveImage(string imageName);
}