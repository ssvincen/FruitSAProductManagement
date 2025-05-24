namespace FruitSA.Web.Models.Account
{
    public class JwtResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
