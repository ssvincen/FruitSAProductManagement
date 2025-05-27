namespace FruitSA.Web.Models.Account
{
    public class JwtResponse : ApiResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
