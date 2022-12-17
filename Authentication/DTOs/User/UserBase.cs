using static Authentication.Common.Enum;

namespace Authentication.DTOs.User
{
    public class UserBase
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }
    }
}
