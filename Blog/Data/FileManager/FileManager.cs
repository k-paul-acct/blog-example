namespace Blog.Data.FileManager;

public class FileManager : IFileManager
{
    private readonly string _imagePath;

    public FileManager(IConfiguration configuration)
    {
        _imagePath = configuration["Path:Images"];
    }

    public FileStream GetFileStream(string fileName)
    {
        return new FileStream(Path.Combine(_imagePath, fileName), FileMode.Open, FileAccess.Read);
    }

    public async Task<string> SaveFile(IFormFile file)
    {
        var saveDir = Path.Combine(_imagePath);
        if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"img-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}{extension}";

        await using var fileStream = new FileStream(Path.Combine(saveDir, fileName), FileMode.Create);
        await file.CopyToAsync(fileStream);

        return fileName;
    }
}