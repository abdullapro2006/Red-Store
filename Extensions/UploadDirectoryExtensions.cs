using Microsoft.CodeAnalysis.CSharp.Syntax;
using RedStore.Contracts;
using RedStore.Exceptions;

namespace RedStore.Extensions;

public static class UploadDirectoryExtensions
{
    public static string GetAbsolutePath(this UploadDirectory uploadDir)
    {
        switch (uploadDir)
        {
            case UploadDirectory.Products:
                return @"C:\\Users\\Abdullah Manafli\\Documents\\RedStore\\RedStore\\wwwroot\\custom-images\\products";
            default:
                throw new UploadDirectoryException("Upload path not found", uploadDir);
        }
    }

    public static string GetUrl(this UploadDirectory uploadDir)
    {
        switch (uploadDir)
        {
            case UploadDirectory.Products:
                return "/custom-images/products";
            default:
                throw new UploadDirectoryException("Upload path not found", uploadDir);
        }
    }

    public static string GetAbsolutePath(this UploadDirectory uploadDir, string fileName)
    {
        return Path.Combine(GetAbsolutePath(uploadDir), fileName);
    }


    public static string GetUrl(this UploadDirectory uploadDir, string fileName)
    {
        return $"{GetUrl(uploadDir)}/{fileName}";
    }
}
