namespace Onion.WebApi.Models;

public class ErrorRes
{
    public ErrorRes(int statusCode, string message, string details = null)
    {
        StatusCode = statusCode;
        Message = message;
        ServerDetails = details;
    }

    public ErrorRes()
    {
    }

    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string ServerDetails { get; set; }
}
