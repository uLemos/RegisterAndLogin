namespace WebApp.Identity
{
    public class MyUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizeUserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
