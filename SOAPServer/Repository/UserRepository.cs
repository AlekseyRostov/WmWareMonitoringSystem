using System;
using System.Collections.Generic;
using System.Linq;
using Model.Entity;

namespace SOAPServer.Repository
{
    public static class UserRepository
    {
        private static List<User> _users = new List<User>()
                                    {
                                        new User() {Name = "Aleksey"}
                                    };

        public static User GetById(Guid userId)
        {
            return _users.FirstOrDefault(x => x.Id == userId);
        }
    }
}