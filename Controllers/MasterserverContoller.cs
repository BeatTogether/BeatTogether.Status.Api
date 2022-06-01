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
    //TODO cache values that are fetched often and only get the values from the master/dedi servers if the chached values are over 2 seconds old or something, will reduce traffic to the servers
    [ApiController]
    [Route("[controller]")]
    public class MasterserverController : ControllerBase
    {
        private readonly MasterserverConfiguration _configuration;
        private readonly IMatchmakingService _matchmakingService;
        private readonly IApiInterface _apiInterface;


        private const string FullAccess = "FULLACCESS";
        private const string CreateAndDestryPermanantServers = "MIDACCESS";
        //private const string CreateBasicServers = "BASICACCESS";


        public MasterserverController(IOptionsSnapshot<MasterserverConfiguration> configuration, IMatchmakingService matchmakingService, IApiInterface apiInstance)
        {
            _configuration = configuration.Value;
            _matchmakingService = matchmakingService;
            _apiInterface = apiInstance;
        }

        [HttpGet(Name = "GetMasterserverController")]
        public async Task<MasterServerData> Get()
        {
            var PublicCount = await _apiInterface.GetPublicServerCount(new GetPublicServerCountRequest());
            var AllCount = await _apiInterface.GetServerCount(new GetServerCountRequest());
            return new MasterServerData(
                _configuration.Endpoint,
                PublicCount.Servers,
                AllCount.Severs
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

        [HttpGet("GetList/Secret/all/{AccessToken}")]
        public async Task<ActionResult<string[]>> GetAllSecretList(string AccessToken)
        {
            if(AccessToken != FullAccess)
                return Unauthorized();
            ServerSecretListResponse Response = await _apiInterface.GetServerSecretsList(new GetServerSecretsListRequest());
            return Response.Secrets;
        }

        [HttpGet("GetList/Secret/public")]
        public async Task<string[]> GetPublicSecretList()
        {
            PublicServerSecretListResponse Response = await _apiInterface.GetPublicServerSecrets(new GetPublicServerSecretsListRequest());
            return Response.Secrets;
        }

        [HttpGet("GetList/simpleserver/all/{AccessToken}")]
        public async Task<ActionResult<SimpleServer[]>> GetAllSimpleServerList(string AccessToken)
        {
            if (AccessToken != FullAccess)
                return Unauthorized();
            ServerListResponse Response = await _apiInterface.GetServers(new GetSimpleServersRequest());
            return Response.Servers;
        }

        [HttpGet("GetList/simpleserver/public")]
        public async Task<SimpleServer[]> GetPublicSimpleServerList()
        {
            PublicServerListResponse Response = await _apiInterface.GetPublicServers(new GetPublicSimpleServersRequest());
            return Response.Servers;
        }

        [HttpGet("GetTemplate")]
        public RegularServerTemplate GetServerTemplate()
        {
            RegularServerTemplate Response = new(
                "ManagerId",
                new GameplayServerConfiguration(
                    5,
                    DiscoveryPolicy.Public,
                    InvitePolicy.AnyoneCanInvite,
                    GameplayServerMode.Managed,
                    SongSelectionMode.ManagerPicks,
                    GameplayServerControlSettings.All),
                false,
                10000,
                "ServerName",
                MasterServer.Interface.ApiInterface.Enums.BeatmapDifficultyMask.All,
                MasterServer.Interface.ApiInterface.Enums.GameplayModifiersMask.None,
                new SongPackMask(0,0));
            return Response;
        }

        [HttpPost("CreateServer")]
        public async Task<ActionResult<SimpleServer>> CreateRegularServer(RegularServerTemplate RegularServerTemplate)
        {
            //if(!(AccessToken == CreateAndDestryPermanantServers || AccessToken == FullAccess))
            //    return Unauthorized();
            //Console.WriteLine(createServerRequest.ToString());
            CreatedServerResponse response = await _apiInterface.CreateServer(new CreateServerRequest(
                RegularServerTemplate.ManagerId,
                RegularServerTemplate.GameplayServerConfiguration,
                RegularServerTemplate.PermanantManager,
                RegularServerTemplate.Timeout,
                RegularServerTemplate.ServerName,
                RegularServerTemplate.GameplayServerConfiguration.DiscoveryPolicy == DiscoveryPolicy.Public,
                RegularServerTemplate.BeatmapDifficultyMask,
                RegularServerTemplate.GameplayModifiersMask,
                RegularServerTemplate.SongPackMask,
                "",
                ""));
            if (!response.Success)
                return BadRequest();
            ServerFromSecretResponse serverFromSecretResponse = await _apiInterface.GetServerFromSecret(new GetServerFromSecretRequest(response.Secret));
            if (!serverFromSecretResponse.Success)
                return NotFound();
            return serverFromSecretResponse.Server;
        }
    }
}
