using AspNetCore.Identity.MongoDbCore.Models;

namespace Chat_Application.Models.Identity
{
    public class ApplicationUser : MongoIdentityUser
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; } 
    }
}
