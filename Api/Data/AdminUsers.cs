using Rtgsms.Models;
using RtgsmsApi.Constants;

namespace RtgsmsApi.Data
{
    public class AdminUsers
    {
        public static AppUser Admin = new AppUser()
        {
            PasswordHash = "2288Shiks.",
            UserName = "Humphryshikunzi",
            Email = "humphry.shikunzi@outlook.com",
            Role = Roles.Admin
        };
    }
}
