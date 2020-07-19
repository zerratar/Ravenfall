using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Managers
{
    public interface IUserManager
    {
        User Get(string username);
    }

    public class StreamBotManager : IStreamBotManager
    {
        private readonly IGameSessionManager sessionManager;
        private readonly object mutex = new object();

        private readonly List<IStreamBot> bots = new List<IStreamBot>();

        public StreamBotManager(IGameSessionManager sessionManager)
        {
            this.sessionManager = sessionManager;
        }

        public void Add(IStreamBot bot)
        {
            lock (mutex)
            {
                bots.Add(bot);

                // get all unmonitored sessions (sessions without bots)
                // and assign the new bot to those sessions.

                IReadOnlyList<IGameSession> sessions = sessionManager.GetUnmonitoredSessions();
                foreach (var session in sessions)
                {
                    session.AssignBot(bot);
                }
            }
        }

        public IStreamBot GetMostAvailable()
        {
            lock (mutex)
            {
                if (bots.Count == 0) return null;
                if (bots.Count == 1) return bots[0];
                return bots
                    .OrderBy(x => x.AvailabilityScore)
                    .FirstOrDefault();
            }
        }

        public void Remove(IStreamBot bot)
        {
            lock (mutex)
            {
                bots.Remove(bot);

                // would be better to do: bot.GetAllSessions();
                // but the bot has no references to the session right now

                var sessions = sessionManager.GetAll();
                foreach (var session in sessions)
                {
                    session.UnassignBot(bot);
                }
            }
        }
    }
}
