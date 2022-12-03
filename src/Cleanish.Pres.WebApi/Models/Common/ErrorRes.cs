namespace Cleanish.Pres.WebApi.Models.Common;

internal class ErrorRes
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string Details { get; set; }

    public ErrorRes()
    {
    }

    public ErrorRes(int statusCode, string message, string details = null)
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }

}
