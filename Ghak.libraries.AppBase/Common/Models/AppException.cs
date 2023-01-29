namespace Ghak.libraries.AppBase.Common.Models;

public class AppException : Exception
{
    public AppException()
    {
    }

    public AppException(string message, int code, string errorTitle = "_") : base(message)
    {
        ErrorTitle = errorTitle;
        ErrorCode = code;
    }

    public string ErrorTitle { get; set; }
    public int ErrorCode { get; set; }
}