using System;
using System.Collections.Concurrent;
using System.Linq;
using log4net;
using Model.Entity;

namespace SOAPServer.Cache
{
    public static class DataCache
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DataCache));

        public static ConcurrentBag<RegisteredUser> RegisteredUsers { get; private set; }
        
        public static void InitDataCache()
        {
            if (RegisteredUsers != null)
                throw new Exception("Кэш уже был инициализирован");

            RegisteredUsers = new ConcurrentBag<RegisteredUser>();
            
        }

        public static RegisteredUser RegisterUser(User user, string computerName)
        {
            var registerUser = GetRegisterUser(user);
            if (registerUser == null)
            {
                RegisteredUsers.Add(new RegisteredUser
                {
                    User = user,
                    ComputerName = computerName,
                    Created = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    SessionGuid = Guid.NewGuid()
                });
            }
            return GetRegisterUser(user);
        }

        public static RegisteredUser GetRegisterUser(User user)
        {
            return RegisteredUsers.FirstOrDefault(el => el.User.Id == user.Id);
        }

        public static RegisteredUser GetOrCreateRegisterUser(User user, string computerName)
        {
            return GetRegisterUser(user) ?? RegisterUser(user, computerName);
        }

        public static RegisteredUser GetSession(Guid sessionGuid)
        {
            RegisteredUser user = RegisteredUsers.FirstOrDefault(el => el.SessionGuid == sessionGuid);

            if (user != null)
                return user;

            throw new SessionNotFoundException("Не найдена сессия с идентификатором " + sessionGuid);
        }

        public static User GetUser(Guid sessionId)
        {
            var session = RegisteredUsers.FirstOrDefault(s => s.SessionGuid == sessionId);

            return session?.User;
        }
        
        public static void ResetCacheUser(Guid userId)
        {
            try
            {
                foreach (var regUser in RegisteredUsers
                    .Where(x => x.User != null && x.User.Id == userId)
                    .ToArray())
                {
                    regUser.ResetCache();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

        }
    }
}