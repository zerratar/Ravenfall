using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

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
        public int Level { get; set; }
        public int Health { get; set; }
        public int Endurance { get; set; }
        public Attributes Attributes { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
        public Appearance Appearance { get; set; }
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
        public bool Running { get; set; }
    }

    public class PlayerMoveResponse
    {
        public const short OpCode = 5;
        public int PlayerId { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
        public bool Running { get; set; }
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
        public bool Static { get; set; }
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
        public string Skill { get; set; }
        public int Level { get; set; }
        public decimal Experience { get; set; }
    }

    public class PlayerLevelUp
    {
        public const short OpCode = 15;
        public int PlayerId { get; set; }
        public string Skill { get; set; }
        public int GainedLevels { get; set; }
    }

    public class MyPlayerAdd
    {
        public const short OpCode = 16;
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public decimal Experience { get; set; }
        public int Health { get; set; }
        public int Endurance { get; set; }
        public Vector3 Position { get; set; }
        public Professions Professions { get; set; }
        public Attributes Attributes { get; set; }
        public Appearance Appearance { get; set; }
        public int[] InventoryItemId { get; set; }
        public long[] InventoryItemAmount { get; set; }
        public long Coins { get; set; }
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

    public class UserPlayerCreate
    {
        public const short OpCode = 19;
        public string Name { get; set; }
        public Appearance Appearance { get; set; }
    }

    public class UserPlayerSelect
    {
        public const short OpCode = 20;
        public int PlayerId { get; set; }
        public string SessionKey { get; set; }
    }

    public class UserPlayerDelete
    {
        public const short OpCode = 21;
        public int PlayerId { get; set; }
    }

    public class UserPlayerList
    {
        public const short OpCode = 22;
        public SelectablePlayer[] Players { get; set; }

        public Player[] GetPlayers()
        {
            if (Players == null || Players.Length == 0)
                return new Player[0];

            var players = new Player[Players.Length];
            for (var i = 0; i < Players.Length; ++i)
            {
                players[i] = new Player
                {
                    Id = Players[i].Id,
                    Name = Players[i].Name,
                    Level = Players[i].Level,
                    Appearance = Players[i].Appearance,
                    Session = Players[i].Session
                };
            }
            return players;
        }
        public class SelectablePlayer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Level { get; set; }
            public Appearance Appearance { get; set; }
            public SessionInfo Session { get; set; }
        }
    }

    public class ConnectionKillSwitch
    {
        public const short OpCode = 24;
        public int Reason { get; set; }
    }

    public class ChatMessage
    {
        public const short OpCode = 25;
        public int ChannelId { get; set; }
        public int PlayerId { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
    }

    public class PlayerInventory
    {
        public const short OpCode = 26;
        public int PlayerId { get; set; }
        public int[] ItemId { get; set; }
        public long[] Amount { get; set; }
        public long Coins { get; set; }
    }


    public class NpcAdd
    {
        public const short OpCode = 27;
        public int ServerId { get; set; }
        public int NpcId { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int Endurance { get; set; }
        public Attributes Attributes { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Destination { get; set; }
    }

    public class NpcRemove
    {
        public const short OpCode = 28;
        public int NpcServerId { get; set; }
    }

    public class NpcUpdate
    {
        public const short OpCode = 29;
        public int ServerId { get; set; }
        public int NpcId { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
    }

    public class NpcMove
    {
        public const short OpCode = 30;
        public int ServerId { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
        public bool Running { get; set; }
    }

    public class PlayerNpcActionRequest
    {
        public const short OpCode = 31;
        public int NpcServerId { get; set; }
        public int ActionId { get; set; }
        public int ParameterId { get; set; }
    }

    public class PlayerPlayerActionRequest
    {
        public const short OpCode = 32;
        public int PlayerId { get; set; }
        public int ActionId { get; set; }
        public int ParameterId { get; set; }
    }

    public class PlayerNpcActionResponse
    {
        public const short OpCode = 33;
        public int PlayerId { get; set; }
        public int NpcServerId { get; set; }
        public int ActionId { get; set; }
        public int ParameterId { get; set; }
        public byte Status { get; set; }
    }

    public class NpcTradeOpenWindow
    {
        public const short OpCode = 34;
        public int PlayerId { get; set; }
        public int NpcServerId { get; set; }
        public string ShopName { get; set; }
        public int[] ItemId { get; set; }
        public int[] ItemPrice { get; set; }
        public int[] ItemStock { get; set; }
    }

    public class NpcTradeUpdateStock
    {
        public const short OpCode = 35;
        public int PlayerId { get; set; }
        public int NpcServerId { get; set; }
        public int[] ItemId { get; set; }
        public int[] ItemPrice { get; set; }
        public int[] ItemStock { get; set; }
    }

    public class NpcTradeSellItem
    {
        public const short OpCode = 36;
        public int NpcServerId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }
    }

    public class NpcTradeBuyItem
    {
        public const short OpCode = 37;
        public int NpcServerId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }
    }

    public class PlayerCoinsUpdate
    {
        public const short OpCode = 38;
        public int PlayerId { get; set; }
        public long Coins { get; set; }
    }

    public class PlayerHealthChange
    {
        public const short OpCode = 39;
        public int TargetPlayerId { get; set; }
        public int PlayerId { get; set; }
        public int Delta { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
    }

    public class NpcHealthChange
    {
        public const short OpCode = 40;
        public int NpcServerId { get; set; }
        public int PlayerId { get; set; }
        public int Delta { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
    }

    public class NpcDeath
    {
        public const short OpCode = 41;
        public int NpcServerId { get; set; }
        public int PlayerId { get; set; }
    }

    public class NpcRespawn
    {
        public const short OpCode = 42;
        public int NpcServerId { get; set; }
        public int PlayerId { get; set; }
    }

    public class NpcAnimationStateUpdate
    {
        public const short OpCode = 43;
        public int NpcServerId { get; set; }
        public string AnimationState { get; set; }
        public bool Enabled { get; set; }
        public bool Trigger { get; set; }
        public int ActionNumber { get; set; }
    }

}
