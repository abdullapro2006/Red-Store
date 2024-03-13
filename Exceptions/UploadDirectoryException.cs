using RedStore.Contracts;

namespace RedStore.Exceptions;

public class UploadDirectoryException : ApplicationException
{
    public UploadDirectory Path { get; set; }
    public UploadDirectoryException(string message, UploadDirectory uploadDir)
        : base("Specified path not found")
    {
        Path = uploadDir;
    }
}
