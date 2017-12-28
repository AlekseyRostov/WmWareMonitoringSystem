using System.Data.Entity;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OwinWebApiSelfHost.Model
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("MyDatabase") { }


        static ApplicationDbContext()
        {
            Database.SetInitializer(new ApplicationDbInitializer());
        }


        // Add a static Create() method:
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        // We still need a DbSet for our Companies 
        // (and any other domain objects):
        public IDbSet<Company> Companies { get; set; }
    }


    public class ApplicationDbInitializer: DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override async void Seed(ApplicationDbContext context)
        {           
            // Set up two initial users with different role claims:
            var admin = new ApplicationUser
            {
                Email = "admin@example.com",
                UserName = "admin"
            };
            var user = new ApplicationUser
            {
                Email = "user@example.com",
                UserName = "user"
            };

            // Introducing...the UserManager:
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var result1 = await manager.CreateAsync(admin, "AdminPassword");
            var result2 = await manager.CreateAsync(user, "UserPassword");

            // Add claims for user #1:
            await manager.AddClaimAsync(admin.Id, new Claim(ClaimTypes.Name, admin.UserName));
            await manager.AddClaimAsync(admin.Id, new Claim(ClaimTypes.Role, "Admin"));

            // Add claims for User #2:
            await manager.AddClaimAsync(user.Id, new Claim(ClaimTypes.Name, user.UserName));
            await manager.AddClaimAsync(user.Id, new Claim(ClaimTypes.Role, "User"));
        }
    }
}
