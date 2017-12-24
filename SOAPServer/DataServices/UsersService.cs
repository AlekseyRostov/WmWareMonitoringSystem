using System;
using Model.Entity;
using SOAPServer.Repository;

namespace SOAPServer.DataServices
{
    internal class UsersService
    {
        public static User GetUserById(Guid userId)
        {
            return UserRepository.GetById(userId);
        }
        
    }
}