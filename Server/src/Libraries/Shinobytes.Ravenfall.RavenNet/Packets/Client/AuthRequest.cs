using System;
using System.Collections.Generic;
using System.Text;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public enum AuthResult
    {
        Success = 0,
        InvalidPassword = 1,
        TemporaryDisabled = 2,
        PermanentlyDisabled = 3
    }

    public class AuthRequest
    {
        public const short OpCode = 0;
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientVersion { get; set; }
    }

    public class AuthResponse
    {
        public const short OpCode = 1;
        public int Status { get; set; }
        public byte[] SessionKeys { get; set; }
    }


    public class Dummy
    {
        public const short OpCode = 999;
        public int Test { get; set; }
    }
}
