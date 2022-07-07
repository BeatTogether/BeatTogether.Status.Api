using BeatTogether.Api.Cache.Abstractions;
using BeatTogether.Api.Configuration;
using BeatTogether.MasterServer.Interface.ApiInterface;
using BeatTogether.MasterServer.Interface.ApiInterface.Models;
using BeatTogether.MasterServer.Interface.ApiInterface.Requests;
using BeatTogether.MasterServer.Interface.ApiInterface.Responses;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace BeatTogether.Api.Cache
{
    public class MasterServerCache : IMasterServerCache
    {
        private DateTime LastMServerRequest = DateTime.MinValue;
        private ConcurrentDictionary<string, SimpleServer> AllServerCache = new();
        private ConcurrentDictionary<string, SimpleServer> PublicServerCache = new();
        private ConcurrentDictionary<string, string> ServerCodeToSecretCache = new();
        private ServerNode[] ServerNodeCache = Array.Empty<ServerNode>();
        private int PlayerCount;
        private int PublicServerCount;
        private int ServerCount;
        private long TotalJoins = 0;

        private readonly IApiInterface _apiInterface;
        private readonly MasterserverConfiguration _configuration;

        public MasterServerCache(IApiInterface msInterface, IOptionsSnapshot<MasterserverConfiguration> config)
        {
            _apiInterface = msInterface;
            _configuration = config.Value;
        }


        public async Task TryUpdateCache()
        {
            if (LastMServerRequest.AddSeconds(-_configuration.MinSecondsBetweenMSRequests) > DateTime.Now)
                await UpdateCache();
            return;
        }

        private async Task UpdateCache() //TODO i kinda hate how i made it just fetch everything. needs refining but that can be done later or something
        {
            ServerNodeCache = (await _apiInterface.GetNodes(new GetServerNodesRequest())).Nodes;
            ServerListResponse serverListResponse = await _apiInterface.GetServers(new GetServersRequest());
            AllServerCache.Clear();
            foreach (var item in serverListResponse.Servers)
            {
                AllServerCache.TryAdd(item.Secret, item);
                ServerCodeToSecretCache.TryAdd(item.Code, item.Secret);
            }
            PublicServerCache = (ConcurrentDictionary<string, SimpleServer>)AllServerCache.Where(value => value.Value.GameplayServerConfiguration.DiscoveryPolicy == MasterServer.Interface.ApiInterface.Enums.DiscoveryPolicy.Public);
            ServerCount = AllServerCache.Count;
            PublicServerCount = PublicServerCache.Count();
            int count = 0;
            foreach (var item in AllServerCache.Values)
            {
                count += item.CurrentPlayerCount;
            }
            PlayerCount = count;
            TotalJoins = (await _apiInterface.GetPlayerJoins(new GetPlayerJoins())).Joins;
            return;
        }

        public async Task<SimpleServer?> GetServerFromSecret(string secret)
        {
            await TryUpdateCache();
            return AllServerCache.TryGetValue(secret, out SimpleServer? server) ? server :null;
        }

        public async Task<SimpleServer?> GetServerFromCode(string code)
        {
            await TryUpdateCache();
            if (ServerCodeToSecretCache.TryGetValue(code, out string? secret))
            {
                return AllServerCache.TryGetValue(secret, out SimpleServer? server) ? server : null;
            }
            return null;
        }

        public async Task<SimpleServer?> GetPublicServerFromSecret(string secret)
        {
            await TryUpdateCache();
            return PublicServerCache.TryGetValue(secret, out SimpleServer? server) ? server : null;
        }

        public async Task<SimpleServer?> GetPublicServerFromCode(string code)
        {
            await TryUpdateCache();
            if (ServerCodeToSecretCache.TryGetValue(code, out string? secret))
            {
                return PublicServerCache.TryGetValue(secret, out SimpleServer? server) ? server : null;
            }
            return null;
        }

        public async Task<string[]> GetAllSecrets()
        {
            await TryUpdateCache();
            return AllServerCache.Keys.ToArray();
        }

        public async Task<string[]> GetAllPublicSecrets()
        {
            await TryUpdateCache();
            return PublicServerCache.Keys.ToArray();
        }

        public async Task<string[]> GetAllCodes()
        {
            await TryUpdateCache();
            return ServerCodeToSecretCache.Keys.ToArray();
        }

        public async Task<string[]> GetAllPublicCodes()
        {
            await TryUpdateCache();
            return PublicServerCache.Values.Select(v => { return v.Code; }).ToArray();
        }

        public async Task<SimpleServer[]> GetServers()
        {
            await TryUpdateCache();
            return AllServerCache.Values.ToArray();
        }

        public async Task<SimpleServer[]> GetPublicServers()
        {
            await TryUpdateCache();
            return PublicServerCache.Values.ToArray();
        }

        public async Task<int> GetPlayerCount()
        {
            await TryUpdateCache();
            return PlayerCount;
        }

        public async Task<int> GetPublicServerCount()
        {
            await TryUpdateCache();
            return PublicServerCount;
        }

        public async Task<int> GetAllServerCount()
        {
            await TryUpdateCache();
            return ServerCount;
        }

        public async Task<ServerNode[]> GetNodes()
        {
            await TryUpdateCache();
            return ServerNodeCache;
        }

        public async Task<long> GetTotalJoins()
        {
            await TryUpdateCache();
            return TotalJoins;
        }
    }
}
