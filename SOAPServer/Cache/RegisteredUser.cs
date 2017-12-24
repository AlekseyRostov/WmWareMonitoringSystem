using System;
using Model.Entity;

namespace SOAPServer.Cache
{
    public class RegisteredUser
    {
        #region Cache

        private LazyCache.CachingService _cache = new LazyCache.CachingService();
        private Guid _userId;
        private object _userLock = new object();
        private TimeSpan _cacheDuration = TimeSpan.FromSeconds(20);

        #endregion

        public string ComputerName { get; set; }

        public Guid SessionGuid { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastUpdated { get; set; }

        public User User
        {
            get
            {
                if (_userId == Guid.Empty) return null;
                lock (_userLock)
                {
                    return _cache.GetOrAdd(GetUserKey(_userId), () => GetUserFromRepository(_userId), _cacheDuration);
                }
            }
            set
            {
                lock (_userLock)
                {
                    _userId = value?.Id ?? Guid.Empty;
                }
            }
        }

        private static string GetUserKey(Guid UserId)
        {
            return $"SOAPServer.exe.RegisteredUser.{UserId}";
        }

        public void ResetCache()
        {
            if (_userId == Guid.Empty) return;
            lock (_userLock)
            {
                string key = GetUserKey(_userId);
                _cache.Remove(key);
            }
        }
        
        private User GetUserFromRepository(Guid UserId)
        {
            return DataServices.UsersService.GetUserById(UserId);
        }
    }
}