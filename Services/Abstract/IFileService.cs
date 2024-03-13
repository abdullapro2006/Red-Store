using RedStore.Contracts;

namespace RedStore.Services.Abstract;

public interface IFileService
{
    string Upload(IFormFile file, string path);
    string Upload(IFormFile file, UploadDirectory uploadDir);
    void Delete(string path);
    void Delete(UploadDirectory uploadDir, string fileName);
}