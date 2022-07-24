using BeatTogether.DedicatedServer.Interface.Models;
using BeatTogether.MasterServer.Interface.ApiInterface.Enums;
using System.Net;

namespace BeatTogether.Status.Api.Models
{

    public interface IPlayer
    {
        public string userId { get; }
    }
    public class Player : IPlayer
    {
        public string userId { get; }
        public string userName;
        public IPEndPoint remoteEndpoint;
        public Platform platform;
        public bool isOnline = false;



        public Player(string UserId, string UserName, IPEndPoint RemoteEndpoint, Platform Platform)
        {
            userId = UserId;
            userName = UserName;
            remoteEndpoint = RemoteEndpoint;
            platform = Platform;
        }
    }

    class Connectedplayer : Player
    {
        public AvatarData avatarData;
        public byte connectionId;
        public int sortId;
        public Connectedplayer(Player player, byte ConnectionId, int SortId, AvatarData AvatarData, IPEndPoint RemoteEndpoint) : base(player.userId, player.userName, RemoteEndpoint, player.platform)
        {
            avatarData = AvatarData;
            connectionId = ConnectionId;
            sortId = SortId;
            isOnline = true;
        }

        public Player Trim()
        {
            isOnline = false;
            return this as Player;
        }
    }
}
