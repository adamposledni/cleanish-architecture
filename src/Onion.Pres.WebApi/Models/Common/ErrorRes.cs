namespace Onion.Pres.WebApi.Models.Common;

public class ErrorRes
{
    public ErrorRes(int statusCode, string message, string details = null)
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }

    public ErrorRes()
    {
    }

    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string Details { get; set; }
}
