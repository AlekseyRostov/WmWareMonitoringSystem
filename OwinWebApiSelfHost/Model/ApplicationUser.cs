﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace OwinWebApiSelfHost.Model
{
    public class ApplicationUser : IdentityUser
    {
        // A default Constructor:
        public ApplicationUser()
        {
        }

        public ApplicationUser(string email) : base(email)
        {
            // Use the email for both user name AND email:
            UserName = email;
        }
    }


    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            return new ApplicationUserManager(
                new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
        }
    }
}