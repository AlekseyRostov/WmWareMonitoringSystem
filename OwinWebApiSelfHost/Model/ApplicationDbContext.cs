using System.Data.Entity;
using System.Security.Claims;

namespace OwinWebApiSelfHost.Model
{
    public class ApplicationDbContext : DbContext
    {
        static ApplicationDbContext()
        {
            Database.SetInitializer(new ApplicationDbInitializer());
        }

        public ApplicationDbContext()
            : base("MyDatabase")
        {
        }

        public IDbSet<Company> Companies { get; set; }
        public IDbSet<MyUser> Users { get; set; }
        public IDbSet<MyUserClaim> Claims { get; set; }
    }

    public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override async void Seed(ApplicationDbContext context)
        {
            context.Companies.Add(new Company {Name = "Microsoft"});
            context.Companies.Add(new Company {Name = "Apple"});
            context.Companies.Add(new Company {Name = "Google"});
            context.SaveChanges();

            // Set up two initial users with different role claims:
            var john = new MyUser {Email = "john@example.com"};
            var jimi = new MyUser {Email = "jimi@Example.com"};

            john.Claims.Add(new MyUserClaim
            {
                ClaimType = ClaimTypes.Name,
                UserId = john.Id,
                ClaimValue = john.Email
            });
            john.Claims.Add(new MyUserClaim
            {
                ClaimType = ClaimTypes.Role,
                UserId = john.Id,
                ClaimValue = "Admin"
            });

            jimi.Claims.Add(new MyUserClaim
            {
                ClaimType = ClaimTypes.Name,
                UserId = jimi.Id,
                ClaimValue = jimi.Email
            });
            jimi.Claims.Add(new MyUserClaim
            {
                ClaimType = ClaimTypes.Role,
                UserId = john.Id,
                ClaimValue = "User"
            });

            var store = new MyUserStore(context);
            await store.AddUserAsync(john, "JohnsPassword");
            await store.AddUserAsync(jimi, "JimisPassword");
        }
    }
}