using Autobus;
using Microsoft.Extensions.Hosting;
using BeatTogether.DedicatedServer.Interface.Events;
using BeatTogether.MasterServer.Interface.ApiInterface.Requests;
using BeatTogether.MasterServer.Interface.ApiInterface.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using BeatTogether.MasterServer.Interface.Events;
using BeatTogether.Status.Api.Models;
using System.Net;

namespace BeatTogether.Status.Api.DediServer
{
    public class ServerCache : IHostedService
    {
        private readonly IAutobus _autobus;
        ConcurrentDictionary<string, IPlayer> Players = new();

        public ServerCache(IAutobus autobus)
        {
            _autobus = autobus;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _autobus.Subscribe<PlayerConnectedToMatchmakingServerEvent>(LogPlayerHandler);
            _autobus.Subscribe<PlayerJoinEvent>(PlayerJoinedDedi);
            _autobus.Subscribe<PlayerLeaveEvent>(PlayerLeftDedi);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _autobus.Unsubscribe<PlayerConnectedToMatchmakingServerEvent>(LogPlayerHandler);
            _autobus.Unsubscribe<PlayerJoinEvent>(PlayerJoinedDedi);
            _autobus.Unsubscribe<PlayerLeaveEvent>(PlayerLeftDedi);

            return Task.CompletedTask;
        }

        private Task LogPlayerHandler(PlayerConnectedToMatchmakingServerEvent PlayerEvent)
        {
            Player Joining = new Player(PlayerEvent.UserId, PlayerEvent.UserName, IPEndPoint.Parse(PlayerEvent.RemoteEndPoint), PlayerEvent.Platform);
            if (!Players.TryAdd(Joining.userId, Joining))
                Players[Joining.userId] = Joining;
            return Task.CompletedTask;
        }

        private Task PlayerJoinedDedi(PlayerJoinEvent PlayerEvent)
        {
            if (Players.TryGetValue(PlayerEvent.UserId, out var P) && P is Player p)
                Players[p.userId] = new Connectedplayer(p, PlayerEvent.ConnectionId, PlayerEvent.SortId, PlayerEvent.AvatarData, IPEndPoint.Parse(PlayerEvent.EndPoint));
            return Task.CompletedTask;
        }

        private Task PlayerLeftDedi(PlayerLeaveEvent PlayerEvent)
        {
            if (Players.TryGetValue(PlayerEvent.UserId, out var Player) && Player is Connectedplayer p)
                Players[p.userId] = p.Trim();
            return Task.CompletedTask;
        }
    }
}
