using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
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

    public class PlayerAdd
    {
        public const short OpCode = 2;
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int CombatLevel { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
    }

    public class PlayerRemove
    {
        public const short OpCode = 3;
        public int PlayerId { get; set; }
    }

    public class PlayerMoveRequest
    {
        public const short OpCode = 4;
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
    }

    public class PlayerMoveResponse
    {
        public const short OpCode = 5;
        public int PlayerId { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
    }

    public class PlayerPositionUpdate
    {
        public const short OpCode = 6;
        public Vector3 Position { get; set; }
    }

    public class PlayerObjectActionRequest
    {
        public const short OpCode = 7;
        public int ObjectServerId { get; set; }
        public int ActionId { get; set; }
        public int ParameterId { get; set; }
    }

    public class PlayerObjectActionResponse
    {
        public const short OpCode = 8;
        public int PlayerId { get; set; }
        public int ObjectServerId { get; set; }
        public int ActionId { get; set; }
        public int ParameterId { get; set; }
        public byte Status { get; set; }
    }

    public class ObjectAdd
    {
        public const short OpCode = 9;
        public int ObjectServerId { get; set; }
        public int ObjectId { get; set; }
        public Vector3 Position { get; set; }
    }

    public class ObjectRemove
    {
        public const short OpCode = 10;
        public int ObjectServerId { get; set; }
    }

    public class ObjectUpdate
    {
        public const short OpCode = 11;
        public int ObjectServerId { get; set; }
        public int ObjectId { get; set; }
        public Vector3 Position { get; set; }
    }

    public class PlayerAnimationStateUpdate
    {
        public const short OpCode = 12;
        public int PlayerId { get; set; }
        public string AnimationState { get; set; }
        public bool Enabled { get; set; }
        public bool Trigger { get; set; }
        public int ActionNumber { get; set; }
    }

    public class PlayerEquipmentStateUpdate
    {
        public const short OpCode = 13;
        public int PlayerId { get; set; }
        public int ItemId { get; set; }
        public bool Equipped { get; set; }
    }

    public class PlayerStatUpdate
    {
        public const short OpCode = 14;
        public int PlayerId { get; set; }
        public int Skill { get; set; }
        public int Level { get; set; }
        public int EffectiveLevel { get; set; }
        public decimal Experience { get; set; }
    }

    public class PlayerLevelUp
    {
        public const short OpCode = 15;
        public int PlayerId { get; set; }
        public int Skill { get; set; }
        public int GainedLevels { get; set; }
    }

    public class MyPlayerAdd
    {
        public const short OpCode = 16;
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public int[] EffectiveLevel { get; set; }
        public decimal[] Experience { get; set; }
    }

    public class PlayerItemAdd
    {
        public const short OpCode = 17;
        public int PlayerId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }
    }

    public class PlayerItemRemove
    {
        public const short OpCode = 18;
        public int PlayerId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }
    }
}
