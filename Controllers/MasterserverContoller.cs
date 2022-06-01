using System;
using System.Net;
using System.Threading.Tasks;
using BeatTogether.Api.Configuration;
//using BeatTogether.Api.Enums;
using BeatTogether.Api.Models;
using BeatTogether.DedicatedServer.Interface;
using BeatTogether.DedicatedServer.Interface.Enums;
using BeatTogether.DedicatedServer.Interface.Models;
using BeatTogether.DedicatedServer.Interface.Requests;
using BeatTogether.DedicatedServer.Interface.Responses;
using BeatTogether.MasterServer.Interface.ApiInterface.Abstractions;
using BeatTogether.MasterServer.Interface.ApiInterface.Models;
using BeatTogether.MasterServer.Interface.ApiInterface.Responses;
using BeatTogether.MasterServer.Interface.ApiInterface.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BeatTogether.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MasterserverController : ControllerBase
    {
        private readonly MasterserverConfiguration _configuration;
        private readonly IMatchmakingService _matchmakingService;
        private readonly IApiInterface _apiInterface;

        public MasterserverController(IOptionsSnapshot<MasterserverConfiguration> configuration, IMatchmakingService matchmakingService, IApiInterface apiInstance)
        {
            _configuration = configuration.Value;
            _matchmakingService = matchmakingService;
            _apiInterface = apiInstance;
        }

        [HttpGet(Name = "GetMasterserverController")]
        public MasterServerData Get()
        {
            int CurrentInstanceCount = 0;
            int CurrentPublicInstanceCount = 0;
            return new MasterServerData(
                _configuration.Endpoint,
                CurrentPublicInstanceCount,
                CurrentInstanceCount
            );
        }
        [HttpGet("test")]
        public string Gettest()
        {
            return "NO";
        }

        [HttpGet("Getserver/simple/secret/{secret}")]
        public async Task<ActionResult<SimpleServer>> GetSimpleServerFromSecret(string secret)
        {
            ServerFromSecretResponse Response = await _apiInterface.GetServerFromSecret(new GetServerFromSecretRequest(secret));
            if (!Response.Success)
                return NotFound();
            return Response.Server;
        }

        [HttpGet("Getserver/simple/code/{code}")]
        public async Task<ActionResult<SimpleServer>> GetSimpleServerFromCode(string code)
        {
            ServerFromCodeResponse Response = await _apiInterface.GetServerFromCode(new GetServerFromCodeRequest(code));
            if (!Response.Success)
                return NotFound();
            return Response.Server;
        }

        [HttpGet("Getserver/advanced/secret/{secret}")]
        public async Task<ActionResult<AdvancedServer>> GetAdvancedServerFromCodeAsync(string secret)
        {
            AdvancedInstanceResponce Response = await _matchmakingService.GetAdvancedInstance(new GetAdvancedInstanceRequest(secret));
            if(!Response.success)
                return NotFound();
            return AdvancedServer.Convert(Response._AdvancedInstance!, secret);
        }


        [HttpGet("GetList/Secret/all")]
        public async Task<string[]> GetAllSecretList()
        {
            ServerSecretListResponse Response = await _apiInterface.GetServerSecretsList(new GetServerSecretsListRequest());
            return Response.Secrets;
        }

        [HttpGet("GetList/Secret/public")]
        public async Task<string[]> GetPublicSecretList()
        {
            PublicServerSecretListResponse Response = await _apiInterface.GetPublicServerSecrets(new GetPublicServerSecretsListRequest());
            return Response.Secrets;
        }

        [HttpGet("GetList/simpleserver/all")]
        public async Task<SimpleServer[]> GetAllSimpleServerList()
        {
            ServerListResponse Response = await _apiInterface.GetServers(new GetSimpleServersRequest());
            return Response.Servers;
        }

        [HttpGet("GetList/simpleserver/public")]
        public async Task<SimpleServer[]> GetPublicSimpleServerList()
        {
            PublicServerListResponse Response = await _apiInterface.GetPublicServers(new GetPublicSimpleServersRequest());
            return Response.Servers;
        }
    }
}
