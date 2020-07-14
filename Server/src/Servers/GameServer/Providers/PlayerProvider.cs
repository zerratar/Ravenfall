using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameServer.Managers
{
    public class PlayerProvider : IPlayerProvider
    {
        private List<Player> players = new List<Player>();
        private readonly object mutex = new object();
        private readonly IPlayerStatsProvider statsProvider;
        private int userIndex = 0;

        public PlayerProvider(IPlayerStatsProvider statsProvider)
        {
            this.statsProvider = statsProvider;
        }

        public Player Get(int playerId)
        {
            lock (mutex) return players.FirstOrDefault(x => x.Id == playerId);
        }

        public bool Remove(int playerId)
        {
            lock (mutex)
            {
                var player = Get(playerId);
                if (player == null) return false;
                return players.Remove(player);
            }
        }

        public void Create(User user, string name, Appearance appearance)
        {
            var player = CreatePlayer(name);
            player.UserId = user.Id;
            if (appearance != null)
            {
                player.Appearance = appearance;
            }
            var newPlayerList = user.Players.ToList();
            newPlayerList.Add(player);
            user.Players = newPlayerList;
        }

        private Player GetOrAddPlayer(string playerName)
        {
            lock (mutex)
            {
                var player = players.FirstOrDefault(x => x.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase));
                if (player != null) return player;
                return CreatePlayer(playerName);
            }
        }

        private Player CreatePlayer(string playerName)
        {
            lock (mutex)
            {
                var id = Interlocked.Increment(ref userIndex);
                var random = new Random();
                var pos = new Vector3((float)random.NextDouble() * 2f, 7.5f, (float)random.NextDouble() * 2f);
                var appearance = GenerateRandomAppearance();

                var level = statsProvider.GetCombatLevel(id);

                var addedPlayer = new Player()
                {
                    Id = id,
                    Name = playerName,
                    CombatLevel = level,
                    Position = pos,
                    Destination = pos,
                    Appearance = appearance
                };

                players.Add(addedPlayer);
                return addedPlayer;
            }
        }

        private Appearance GenerateRandomAppearance()
        {
            var gender = Utility.Random<Gender>();
            var skinColor = Utility.Random("#d6b8ae");
            var hairColor = Utility.Random("#A8912A", "#27ae60", "#2980b9", "#8e44ad");
            var beardColor = Utility.Random("#A8912A", "#27ae60", "#2980b9", "#8e44ad");
            return new Appearance
            {
                Gender = gender,
                SkinColor = skinColor,
                HairColor = hairColor,
                BeardColor = beardColor,
                StubbleColor = skinColor,
                WarPaintColor = hairColor,
                EyeColor = Utility.Random("#000000", "#c0392b", "#2c3e50"),
                Eyebrows = Utility.Random(0, gender == Gender.Male ? 10 : 7),
                Hair = Utility.Random(0, 38),
                FacialHair = gender == Gender.Male ? Utility.Random(0, 18) : -1,
                Head = Utility.Random(0, 23),
                HelmetVisible = true
            };
        }

        public IReadOnlyList<Player> GetAll()
        {
            lock (mutex) return players.ToList();
        }

        public IReadOnlyList<Player> GetPlayers(User user)
        {
            lock (mutex) return players.Where(x => x.UserId == user.Id).ToList();
        }
    }
}
