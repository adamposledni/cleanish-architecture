namespace Onion.WebApi.Models
{
    public class ErrorRes
    {
        public ErrorRes(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
