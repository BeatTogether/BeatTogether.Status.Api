using System;
using System.Threading.Tasks;
using BeatTogether.Api.Configuration;
using BeatTogether.Api.Models;
using BeatTogether.DedicatedServer.Interface;
using BeatTogether.DedicatedServer.Interface.Requests;
using BeatTogether.DedicatedServer.Interface.Responses;
using BeatTogether.MasterServer.Interface.ApiInterface.Enums;
using BeatTogether.MasterServer.Interface.ApiInterface.Models;
using BeatTogether.MasterServer.Interface.ApiInterface.Responses;
using BeatTogether.MasterServer.Interface.ApiInterface.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Autobus;
using BeatTogether.MasterServer.Interface.ApiInterface;

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
        private readonly IAutobus _autobus;
        //private readonly IHostedClient

        public MasterserverController(IOptionsSnapshot<MasterserverConfiguration> configuration, IMatchmakingService matchmakingService, IApiInterface apiInstance, IAutobus autobus)
        {
            _configuration = configuration.Value;
            _matchmakingService = matchmakingService;
            _apiInterface = apiInstance;
            _autobus = autobus;
        }

        [HttpGet(Name = "GetMasterserverController")]
        public async Task<MasterServerData> Get()
        {
            var PublicCount = await _apiInterface.GetPublicServerCount(new GetPublicServerCountRequest());
            var AllCount = await _apiInterface.GetServerCount(new GetServerCountRequest());
            return new MasterServerData(
                _configuration.Endpoint,
                PublicCount.Servers,
                AllCount.Servers
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
            if (!Response.Success)
                return NotFound();
            return AdvancedServer.Convert(Response._AdvancedInstance!, secret);
        }

        [HttpGet("GetList/Secret/all/{AccessToken}")]
        public async Task<ActionResult<string[]>> GetAllSecretList(string AccessToken)
        {
            if (AccessToken != _configuration.FullAccess)
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
            if (AccessToken != _configuration.FullAccess)
                return Unauthorized();
            ServerListResponse Response = await _apiInterface.GetServers(new GetSimpleServersRequest());
            return Response.Servers;
        }

        [HttpGet("GetList/simpleserver/public")]
        public async Task<ActionResult<SimpleServer[]>> GetPublicSimpleServerList(string AccessToken)
        {
            if (AccessToken != _configuration.FullAccess)
                return Unauthorized();
            PublicServerListResponse Response = await _apiInterface.GetPublicServers(new GetPublicSimpleServersRequest());
            return Response.Servers;
        }

        [HttpGet("GetList/players/{AccessToken}")]
        public async Task<MServerPlayer[]> GetPlayerList()
        {
            PlayersFromMasterServerResponse Response = await _apiInterface.GetAllPlayers(new PlayersFromMasterServerRequest());
            return Response.ServerPlayers;
        }

        [HttpGet("Test/GetTemplate")]
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
                BeatmapDifficultyMask.All,
                GameplayModifiersMask.None,
                new SongPackMask(0, 0));
            return Response;
        }

        [HttpGet("GetPublicServerCount")]
        public async Task<int> GetPublicServerCount()
        {
            MasterServer.Interface.ApiInterface.Responses.PublicServerCountResponse response = await _apiInterface.GetPublicServerCount(new GetPublicServerCountRequest());
            return response.Servers;
        }

        [HttpGet("GetServerCount")]
        public async Task<int> GetServerCount()
        {
            MasterServer.Interface.ApiInterface.Responses.ServerCountResponse response = await _apiInterface.GetServerCount(new GetServerCountRequest());
            return response.Servers;
        }

        [HttpGet("Getserver/advanced/secret/{secret}/players/simple")]
        public async Task<ActionResult<DedicatedServer.Interface.Models.SimplePlayer[]>> GetSimplePlayersList(string secret)
        {
            SimplePlayersListResponce response = await _matchmakingService.GetSimplePlayerList(new GetPlayersSimpleRequest(secret))!;
            if(!response.Success)
                return NotFound();
            return response.SimplePlayers!;
        }

        [HttpGet("Getserver/advanced/secret/{secret}/players/advanced/{AccessToken}/")]
        public async Task<ActionResult<DedicatedServer.Interface.Models.AdvancedPlayer[]>> GetAdvancedPlayersList(string secret, string AccessToken)
        {
            if (!(AccessToken == _configuration.FullAccess))
                return Unauthorized();
            AdvancedPlayersListResponce response = await _matchmakingService.GetAdvancedPlayerList(new GetPlayersAdvancedRequest(secret))!;
            if (!response.Success)
                return NotFound();
            return response.AdvancedPlayers!;
        }

        [HttpGet("Getserver/advanced/secret/{secret}/players/advanced/{PlayerId}/{AccessToken}/")]
        public async Task<ActionResult<DedicatedServer.Interface.Models.AdvancedPlayer>> GetAdvancedPlayerList(string secret,string PlayerId, string AccessToken)
        {
            if (!(AccessToken == _configuration.FullAccess))
                return Unauthorized();
            AdvancedPlayerResponce response = await _matchmakingService.GetAdvancedPlayer(new GetPlayerAdvancedRequest(secret, PlayerId))!;
            if (!response.Success)
                return NotFound();
            return response.AdvancedPlayer!;
        }

        [HttpDelete("Getserver/advanced/secret/{secret}/players/kick/{PlayerId}/{AccessToken}/")]
        public async Task<IActionResult> RemovePlayer(string secret, string PlayerId, string AccessToken)
        {
            if (!(AccessToken == _configuration.FullAccess))
                return Unauthorized();
            KickPlayerResponse response = await _matchmakingService.KickPlayer(new KickPlayerRequest(secret, PlayerId))!;
            if (!response.Success)
                return NotFound();
            return NoContent();
        }

        [HttpPost("CreateServer")]
        public async Task<ActionResult<SimpleServer>> CreateRegularServer(RegularServerTemplate RegularServerTemplate)
        {
            CreateServerRequest request = new(
                RegularServerTemplate.ManagerId,
                RegularServerTemplate.GameplayServerConfiguration,
                RegularServerTemplate.PermanentManager,
                Math.Min(Math.Max(RegularServerTemplate.Timeout, 0), 900),
                RegularServerTemplate.ServerName,
                RegularServerTemplate.BeatmapDifficultyMask,
                RegularServerTemplate.GameplayModifiersMask,
                RegularServerTemplate.SongPackMask);
            //Console.WriteLine(request.ToString());
            CreatedServerResponse response = await _apiInterface.CreateServer(request);
            if (!response.Success)
                return BadRequest();
            ServerFromSecretResponse serverFromSecretResponse = await _apiInterface.GetServerFromSecret(new GetServerFromSecretRequest(response.Secret));
            if (!serverFromSecretResponse.Success)
                return NotFound();
            return serverFromSecretResponse.Server;
        }

        [HttpPost("CreateAdvancedServer/{AccessToken}/")]
        public async Task<ActionResult<SimpleServer>> CreateAdvancedServer(string AccessToken, AdvancedServerTemplate AdvancedServerTemplate)
        {

            if (!(AccessToken == _configuration.CreateAndDestryPermanentServers || AccessToken == _configuration.FullAccess))
                return Unauthorized();
            float Timeout = Math.Max(AdvancedServerTemplate.Timeout, 0);
            if (AdvancedServerTemplate.PermanentServer)
                Timeout = -1;
            CreateServerRequest request = new(
                AdvancedServerTemplate.ManagerId,
                AdvancedServerTemplate.GameplayServerConfiguration,
                AdvancedServerTemplate.PermanentManager,
                Timeout,
                AdvancedServerTemplate.ServerName,
                AdvancedServerTemplate.BeatmapDifficultyMask,
                AdvancedServerTemplate.GameplayModifiersMask,
                AdvancedServerTemplate.SongPackMask,
                AdvancedServerTemplate.Code,
                AdvancedServerTemplate.Secret);


            CreatedServerResponse response = await _apiInterface.CreateServer(request);
            if (!response.Success)
                return BadRequest();
            ServerFromSecretResponse serverFromSecretResponse = await _apiInterface.GetServerFromSecret(new GetServerFromSecretRequest(response.Secret));
            if (!serverFromSecretResponse.Success)
                return NotFound();
            return serverFromSecretResponse.Server;
        }
    
        [HttpDelete("RemoveServer/code/{AccessToken}/{code}")]
        public async Task<IActionResult> RemoveServerWithCode(string AccessToken, string code)
        {
            if (!(AccessToken == _configuration.CreateAndDestryPermanentServers || AccessToken == _configuration.FullAccess))
                return Unauthorized();
            RemoveCodeServerResponse response = await _apiInterface.RemoveServer(new RemoveCodeServerCodeRequest(code));
            if (!response.Success)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("RemoveServer/secret/{AccessToken}/{secret}")]
        public async Task<IActionResult> RemoveServerWithSecret(string AccessToken, string secret)
        {
            if (!(AccessToken == _configuration.CreateAndDestryPermanentServers || AccessToken == _configuration.FullAccess))
                return Unauthorized();
            RemoveSecretServerResponse response = await _apiInterface.RemoveServer(new RemoveSecretServerRequest(secret));
            if (!response.Success)
                return NotFound();
            return NoContent();
        }

        [HttpPost("Getserver/{secret}/SetBeatmap/{AccessToken}/")]
        public async Task<IActionResult> SetServerBeatmap(string AccessToken, SetInstanceBeatmapRequest setInstanceBeatmapRequest)
        {
            if (!(AccessToken == _configuration.CreateAndDestryPermanentServers || AccessToken == _configuration.FullAccess))
                return Unauthorized();
            SetInstanceBeatmapResponse response = await _matchmakingService.SetInstanceBeatmap(setInstanceBeatmapRequest);
            if (!response.Success)
                return NotFound();
            return Accepted();
        }

        [HttpPost("BeatmapRepository/Requirements/set/{AccessToken}/")]
        public async Task<IActionResult> SetAllowedRequirements(string AccessToken, SetAllowedRequirementsRequest AllowedRequirements)
        {
            if (!(AccessToken == _configuration.FullAccess))
                return Unauthorized();
            SetAllowedRequirementsResponse response = await _matchmakingService.SetAllowedRequirements(AllowedRequirements);
            if (!response.Success)
                return NotFound();
            return Accepted();
        }

        [HttpGet("BeatmapRepository/Requirements/")]
        public async Task<GetAllowedRequirementsResponse> SetAllowedRequirements()
        {
            GetAllowedRequirementsResponse response = await _matchmakingService.GetAllowedRequirements(new GetAllowedRequirementsRequest());
            return response;
        }

        [HttpDelete("BeatmapRepository/ClearCachedBeatmaps/{AccessToken}")]
        public async Task<IActionResult> RemoveCachedBeatmaps(string AccessToken)
        {
            if (!(AccessToken == _configuration.FullAccess))
                return Unauthorized();
            ClearCachedBeatmapsResponse response = await _matchmakingService.ClearCachedBeatmaps(new ClearCachedBeatmapsRequest());
            if (!response.Success)
                return NotFound();
            return NoContent();
        }
    }
}
