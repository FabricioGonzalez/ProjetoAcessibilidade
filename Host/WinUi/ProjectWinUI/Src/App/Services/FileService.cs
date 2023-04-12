using System.IO;
using System.Text;
using System.Text.Json;
using ProjectWinUI.Src.App.Contracts;

namespace ProjectWinUI.Src.App.Services;

public class FileService : IFileService
{
    public T Read<T>(
        string folderPath
        , string fileName
    )
    {
        var path = Path.Combine(path1: folderPath, path2: fileName);
        if (File.Exists(path: path))
        {
            var json = File.ReadAllText(path: path);
            return JsonSerializer.Deserialize<T>(json: json);
        }

        return default;
    }

    public void Save<T>(
        string folderPath
        , string fileName
        , T content
    )
    {
        if (!Directory.Exists(path: folderPath))
        {
            Directory.CreateDirectory(path: folderPath);
        }

        var fileContent = JsonSerializer.Serialize(value: content);
        File.WriteAllText(path: Path.Combine(path1: folderPath, path2: fileName), contents: fileContent
            , encoding: Encoding.UTF8);
    }

    public void Delete(
        string folderPath
        , string fileName
    )
    {
        if (fileName != null && File.Exists(path: Path.Combine(path1: folderPath, path2: fileName)))
        {
            File.Delete(path: Path.Combine(path1: folderPath, path2: fileName));
        }
    }
}