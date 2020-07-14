using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameServer.Managers
{
    public class UserManager : IUserManager
    {
        private readonly List<User> users = new List<User>();
        private readonly object mutex = new object();
        private int userIndex = 0;

        public User Get(string username)
        {
#warning SHOULD NOT ADD A USER AUTOMATICALLY
            return GetOrAddUser(username);
        }

        private User GetOrAddUser(string username)
        {
            lock (mutex)
            {
                var player = users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (player != null) return player;
                return CreateUser(username);
            }
        }

        private User CreateUser(string username)
        {
            lock (mutex)
            {
                var id = Interlocked.Increment(ref userIndex);
                var addedUser = new User
                {
                    Id = id,
                    Username = username,
                    Players = new Player[0]
                };
                users.Add(addedUser);
                return addedUser;
            }
        }
    }
}
