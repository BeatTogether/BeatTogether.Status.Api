using System;
using System.Threading.Tasks;
using BeatTogether.Api.Configuration;
using BeatTogether.Api.Models;
using BeatTogether.DedicatedServer.Interface;
using BeatTogether.MasterServer.Interface.ApiInterface.Enums;
using BeatTogether.MasterServer.Interface.ApiInterface.Models;
using BeatTogether.MasterServer.Interface.ApiInterface.Responses;
using BeatTogether.MasterServer.Interface.ApiInterface.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Autobus;
using BeatTogether.MasterServer.Interface.ApiInterface;
using BeatTogether.Api.Cache.Abstractions;

namespace BeatTogether.Api.Controllers
{
    //TODO cache values that are fetched often and only get the values from the master/dedi servers if the chached values are over 2 seconds old or something, will reduce traffic to the servers
    //TODO can now remove servers though master server and it will close the dedicated server instance, I THINK, needs checking
    [ApiController]
    [Route("[controller]")]
    public class MasterserverController : ControllerBase
    {
        private readonly MasterserverConfiguration _configuration;
        private readonly IMatchmakingService _matchmakingService;
        private readonly IApiInterface _apiInterface;
        private readonly IAutobus _autobus;
        private readonly IMasterServerCache MasterServer;

        public MasterserverController(IOptionsSnapshot<MasterserverConfiguration> configuration, IMatchmakingService matchmakingService, IApiInterface apiInstance, IAutobus autobus, IMasterServerCache masterServer)
        {
            _configuration = configuration.Value;
            _matchmakingService = matchmakingService;
            _apiInterface = apiInstance;
            _autobus = autobus;
            MasterServer = masterServer;
        }

        [HttpGet(Name = "GetMasterserverController")]
        public async Task<MasterServerData> Get()
        {
            var PublicCount = await MasterServer.GetPublicServerCount();
            var AllCount = await MasterServer.GetAllServerCount();
            return new MasterServerData(
                _configuration.Endpoint,
                PublicCount,
                AllCount
            );
        }

        [HttpGet("Info")]
        public Info GetInfo()
        {
            var desc = _configuration.ModInfo;
            return desc;
        }

        [HttpPost("Info/set/{AccessToken}/")]
        public IActionResult SetDescription(string AccessToken, Info NewInfo)
        {
            if (!(AccessToken == _configuration.FullAccess))
                return Unauthorized();
            _configuration.ModInfo = NewInfo;
            return Accepted();
        }


        [HttpGet("Info/InGame")]
        public string GetInGameMessage()
        {
            var InGameMessage = _configuration.InGameMessage;
            return InGameMessage;
        }

        [HttpPost("Info/InGame/set/{AccessToken}/")]
        public IActionResult SetInGameMessage(string AccessToken, string NewMessage)
        {
            if (!(AccessToken == _configuration.FullAccess))
                return Unauthorized();
            _configuration.InGameMessage = NewMessage;
            return Accepted();
        }


        [HttpGet("Info/MaxPlayers")]
        public int GetMaxPlayers()
        {
            int MaxPlayers = _configuration.MaxPlayersPerInstance;
            return MaxPlayers;
        }

        [HttpPost("Info/MaxPlayers/set/{AccessToken}/")]
        public IActionResult SetMaxPlayers(string AccessToken, string MaxPlayers)
        {
            if (!(AccessToken == _configuration.FullAccess))
                return Unauthorized();
            if (int.TryParse(MaxPlayers, out int Max))
            {
                _configuration.MaxPlayersPerInstance = Max;
                return Accepted();
            }
            return BadRequest();
        }

        [HttpGet("test")]
        public string Gettest()
        {
            return "Pink cute";
        }

        [HttpGet("Nodes/{AccessToken}/")]
        public async Task<ActionResult<ServerNode[]>> GetServerNodes(string AccessToken)
        {
            if (!(AccessToken == _configuration.FullAccess))
                return Unauthorized();
            return await MasterServer.GetNodes();
        }

        [HttpGet("Public/Servers")]
        public async Task<ActionResult<SimpleServer[]>> GetPublicServers()
        {
            return await MasterServer.GetPublicServers();
        }
        [HttpGet("Public/Servers/secrets")]
        public async Task<ActionResult<string[]>> GetPublicServerSecrets()
        {
            return await MasterServer.GetAllPublicSecrets();
        }
        [HttpGet("Public/Servers/codes")]
        public async Task<ActionResult<string[]>> GetPublicServerCodes()
        {
            return await MasterServer.GetAllPublicCodes();
        }

        [HttpGet("Servers/{AccessToken}/")]
        public async Task<ActionResult<SimpleServer[]>> GetServers(string AccessToken)
        {
            if (!(AccessToken == _configuration.FullAccess))
                return Unauthorized();
            return await MasterServer.GetServers();
        }
        [HttpGet("Servers/secrets/{AccessToken}/")]
        public async Task<ActionResult<string[]>> GetServerSecrets(string AccessToken)
        {
            if (!(AccessToken == _configuration.FullAccess))
                return Unauthorized();
            return await MasterServer.GetAllSecrets();
        }
        [HttpGet("Servers/codes/{AccessToken}/")]
        public async Task<ActionResult<string[]>> GetServerCodes(string AccessToken)
        {
            if (!(AccessToken == _configuration.FullAccess))
                return Unauthorized();
            return await MasterServer.GetAllCodes();
        }

        [HttpGet("Public/Server/{code}/")]
        public async Task<ActionResult<SimpleServer>> GetPublicServerFromCode(string code)
        {
            SimpleServer? server;
            if (code.Length == 5)
            {
                server = await MasterServer.GetPublicServerFromCode(code);
                if (server == null)
                    return NotFound();
                return server;
            }
            server = await MasterServer.GetPublicServerFromSecret(code);
            if (server == null)
                return NotFound();
            return server;
        }

        [HttpGet("Server/{code}/{AccessToken}/")]
        public async Task<ActionResult<SimpleServer>> GetPublicServerFromCode(string AccessToken, string code)
        {
            if (!(AccessToken == _configuration.FullAccess))
                return Unauthorized();
            SimpleServer? server;
            if (code.Length == 5)
            {
                server = await MasterServer.GetServerFromCode(code);
                if (server == null)
                    return NotFound();
                return server;
            }
            server = await MasterServer.GetServerFromSecret(code);
            if (server == null)
                return NotFound();
            return server;
        }

        [HttpGet("PlayerCount")]
        public async Task<ActionResult<long>> GetPlayerCount()
        {
            return await MasterServer.GetPlayerCount();
        }
        [HttpGet("PublicServerCount")]
        public async Task<ActionResult<long>> GetPublicServerCount()
        {
            return await MasterServer.GetPublicServerCount();
        }
        [HttpGet("ServerCount")]
        public async Task<ActionResult<long>> GetAllServerCount()
        {
            return await MasterServer.GetAllServerCount();
        }
        [HttpGet("GetTotalPlayerJoins")]
        public async Task<ActionResult<long>> GetTotalPlayerJoins()
        {
            return await MasterServer.GetTotalJoins();
        }
    }
}
