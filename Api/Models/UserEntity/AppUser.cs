using Microsoft.AspNetCore.Identity;

namespace Rtgsms.Models
{
    public class AppUser : IdentityUser
    {
        public string Role { get; set; }
        public string AuthKey { get; set; }
        public bool IsSignedIn { get; set; }
        public string ProfileImagePath { get; set; }
        public bool TermsTermsAndConditionsChecked { get; set; } 
    }
}
