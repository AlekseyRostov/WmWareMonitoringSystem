using System.Data.Entity;

namespace OwinWebApiSelfHost.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("MyDatabase")
        {
        }

        public IDbSet<Company> Companies { get; set; }
    }

    public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);
            context.Companies.Add(new Company {Name = "Microsoft"});
            context.Companies.Add(new Company {Name = "Google"});
            context.Companies.Add(new Company {Name = "Apple"});
        }
    }
}