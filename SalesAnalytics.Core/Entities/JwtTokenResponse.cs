namespace SalesAnalytics.Core.Entities
{
    public class JwtTokenResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
