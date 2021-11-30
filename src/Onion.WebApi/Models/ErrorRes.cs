namespace Onion.WebApi.Models
{
    public class ErrorRes
    {
        public ErrorRes(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
