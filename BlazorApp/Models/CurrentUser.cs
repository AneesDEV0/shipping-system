namespace BlazorApp.Models
{
    public class CurrentUser
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
//git config --global --add safe.directory "C:/Users/msi/source/repos/ShippingSolution"
