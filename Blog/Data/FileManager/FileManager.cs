using Microsoft.IdentityModel.Tokens;

namespace Blog.Data.FileManager;

public class FileManager : IFileManager
{
    private readonly string _imagesPath;

    public FileManager(IConfiguration configuration)
    {
        _imagesPath = configuration["Path:Images"];
    }

    public FileStream? GetFileStream(string fileName)
    {
        try
        {
            return new FileStream(Path.Combine(_imagesPath, fileName), FileMode.Open, FileAccess.Read);
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<string> SaveFile(IFormFile file)
    {
        if (!Directory.Exists(_imagesPath)) Directory.CreateDirectory(_imagesPath);

        var extension = Path.GetExtension(file.FileName);
        // Generates 24 * 6 / 8 bytes for 24-length base64url id.
        var buffer = new byte[24 * 6 / 8];
        Random.Shared.NextBytes(buffer);
        var fileId = Base64UrlEncoder.Encode(buffer);
        var fileName = $"{fileId}{extension}";

        await using var fileStream = new FileStream(Path.Combine(_imagesPath, fileName), FileMode.Create);
        await file.CopyToAsync(fileStream);

        return fileName;
    }

    public bool RemoveImage(string imageName)
    {
        var filePath = Path.Combine(_imagesPath, imageName);
        if (File.Exists(filePath)) return false;

        try
        {
            File.Delete(filePath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }

        return true;
    }
}