namespace MudBlazorCrmApp.Services;

public class ImageService(IWebHostEnvironment environment)
{
    private readonly string webRootPath = environment.WebRootPath;

    public static (string, byte[]) GetImageFromDataUri(string dataUri)
    {
        var split = dataUri.Split(',');

        if (split.Length != 2)
        {
            throw new ArgumentException("Data URI format is invalid", nameof(dataUri));
        }

        var header = split[0];
        var payload = split[1];

        var extension = header switch
        {
            "data:image/png;base64" => ".png",
            "data:image/jpeg;base64" => ".jpg",
            _ => null,
        };

        if (string.IsNullOrEmpty(extension))
        {
            throw new ArgumentException("Data URI format is invalid", nameof(dataUri));
        }

        var bytes = Convert.FromBase64String(payload);

        return (extension, bytes);
    }

    public async Task<string> SaveToUploadsAsync(string? extension, Stream image)
    {
        if (string.IsNullOrEmpty(extension) || extension != ".png" && extension != ".jpg")
        {
            throw new ArgumentException("The image must be PNG or JPEG format.", nameof(extension));
        }

        string fileName;
        string filePath;

        var uploadPath = Path.Combine(webRootPath, "upload", "image");

        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        do
        {
            var randomFileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());

            fileName = $"{randomFileName}{extension}";
            filePath = Path.Combine(webRootPath, "upload", "image", fileName);
        }
        while (File.Exists(filePath));

        using FileStream fs = new(filePath, FileMode.CreateNew);
        await image.CopyToAsync(fs);

        Uri imageUri = new($"/upload/image/{fileName}", UriKind.Relative);

        return $"/upload/image/{fileName}";
    }
}
    