namespace Authentication.Common
{
    public class AppSettings
    {
        public string SecretKey { get; set; }
        public string[] AllowedOrigins { get; set; }
    }
}
