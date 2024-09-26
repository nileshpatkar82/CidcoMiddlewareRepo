namespace Cidco.Middleware.Application.Models.Response
{
    public class TokenResponse
    {
        public string email { get; set; }
        public string username { get; set; }
        public DateTime tokenGeneratedDate { get; set; }
        public string token { get; set; }
    }
}
