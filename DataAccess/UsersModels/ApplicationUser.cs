using Microsoft.AspNetCore.Identity;


namespace DataAccess.UsersModel
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }
    }

}