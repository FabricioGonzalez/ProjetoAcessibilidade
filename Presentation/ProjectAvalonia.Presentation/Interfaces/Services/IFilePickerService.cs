namespace ProjectAvalonia.Presentation.Interfaces.Services;
public interface IFilePickerService
{
    Task<string> GetImagesAsync();
    Task<string> GetFolderAsync();
    Task<string> GetSolutionFilesAsync();
    Task<string> GetZipFilesAsync();
}
