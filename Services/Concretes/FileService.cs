﻿using RedStore.Contracts;
using RedStore.Extensions;
using RedStore.Services.Abstract;

namespace RedStore.Services.Concretes;

public class FileService : IFileService
{
    public string Upload(IFormFile file, string path)
    {
        var uniqueFileName = GetUniqueFileName();
        var uploadPath = Path.Combine(path, uniqueFileName);
        using FileStream fileStream = new FileStream(uploadPath, FileMode.Create);
        file.CopyTo(fileStream);

        return uniqueFileName;
    }

  

    public string Upload(IFormFile file, UploadDirectory uploadDir)
    {
        var uniqueFileName = GetUniqueFileName();
        var uploadPath = uploadDir.GetAbsolutePath(uniqueFileName);
        using FileStream fileStream = new FileStream(uploadPath, FileMode.Create);
        file.CopyTo(fileStream);

        return uniqueFileName;
    }

    public void Delete(string path)
    {
        File.Delete(path);
    }


    public void Delete(UploadDirectory uploadDir, string fileName)
    {
        var absolutePath = uploadDir.GetAbsolutePath(fileName);

        Delete(absolutePath);
    }


    private string GetUniqueFileName()
    {
        return Guid.NewGuid().ToString();
    }

}
