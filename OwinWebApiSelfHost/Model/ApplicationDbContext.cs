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
            Database.SetInitializer(
                new ApplicationDbInitializer());
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


    public class ApplicationDbInitializer
        : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected async override void Seed(ApplicationDbContext context)
        {
            context.Companies.Add(new Company { Name = "Microsoft" });
            context.Companies.Add(new Company { Name = "Apple" });
            context.Companies.Add(new Company { Name = "Google" });
            context.SaveChanges();

            // Set up two initial users with different role claims:
            var john = new ApplicationUser
            {
                Email = "john@example.com",
                UserName = "john@example.com"
            };
            var jimi = new ApplicationUser
            {
                Email = "jimi@Example.com",
                UserName = "jimi@example.com"
            };

            // Introducing...the UserManager:
            var manager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var result1 = await manager.CreateAsync(john, "JohnsPassword");
            var result2 = await manager.CreateAsync(jimi, "JimisPassword");

            // Add claims for user #1:
            await manager.AddClaimAsync(john.Id,
                new Claim(ClaimTypes.Name, "john@example.com"));

            await manager.AddClaimAsync(john.Id,
                new Claim(ClaimTypes.Role, "Admin"));

            // Add claims for User #2:
            await manager.AddClaimAsync(jimi.Id,
                new Claim(ClaimTypes.Name, "jimi@example.com"));

            await manager.AddClaimAsync(jimi.Id,
                new Claim(ClaimTypes.Role, "User"));
        }
    }
}
