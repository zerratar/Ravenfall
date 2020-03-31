
using Shinobytes.Ravenfall.RavenNet.Models;
using System;

namespace Shinobytes.Ravenfall.RavenNet.Server.Packets
{
    public class ServerHello
    {
        public const short OpCode = 1000;
        public string Name { get; set; }
        public string ServerIp { get; set; }
        public int ServerPort { get; set; }
    }

    public class AuthChallenge
    {
        public const short OpCode = 1001;
        public Guid CorrelationId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientVersion { get; set; }
    }

    public class ServerStats
    {
        public const short OpCode = 1002;
        public double CpuUsage { get; set; }
        public double MemUsage { get; set; }
        public int PlayerCount { get; set; }
        public int NpcCount { get; set; }
    }

    public class PlayerZoneEnter
    {
        public const short OpCode = 1003;
        public Player Player { get; set; }
    }

    public class PlayerZoneExit
    {
        public const short OpCode = 1004;
        public int PlayerId { get; set; }
    }

    public class PlayerEnterUpdate
    {
        public const short OpCode = 1005;
        public int[] PlayersToUpdate { get; set; }
        public Player Data { get; set; }
    }

    public class PlayerExitUpdate
    {
        public const short OpCode = 1006;
        public int[] PlayersToUpdate { get; set; }
        public Player Data { get; set; }
    }

    public class AuthChallengeResponse
    {

        public const short OpCode = 2001;
        public Guid CorrelationId { get; set; }
        public int Status { get; set; }
        public byte[] SessionKeys { get; set; }
    }

    public class Dummy
    {
        public const short OpCode = 1999;
        public int Test { get; set; }
    }
}
