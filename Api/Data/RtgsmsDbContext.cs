using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RtgsmsApi.Models.Device;
using Rtgsms.Models;

namespace RtgsmsApi.Data
{
    public class RtgsmsDbContext : IdentityDbContext<AppUser>
    {
        public RtgsmsDbContext(DbContextOptions<RtgsmsDbContext> dbContextOptions): base(dbContextOptions)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<SigFoxDevice> SigFoxDevices { get; set; }
    }
}
