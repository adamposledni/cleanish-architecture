namespace Onion.Infrastructure.Security.Jwt
{
    public class TokenProviderSettings
    {
        public string JwtSigningKey { get; set; }
        public double ExpirationTime { get; set; }
    }
}
