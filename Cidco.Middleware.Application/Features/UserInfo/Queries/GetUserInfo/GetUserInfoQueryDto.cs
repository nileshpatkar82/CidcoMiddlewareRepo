namespace Cidco.Middleware.Application.Features.UserInfo.Queries.GetUserInfo
{
    public class GetUserInfoQueryDto
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }

}
