namespace Authentication.DTOs.User
{
    public class UserLogInResponse
    {
        public UserBase User { get; set; }
        public string Token { get; set; }
    }
}
