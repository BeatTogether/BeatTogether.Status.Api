using BeatTogether.Status.Api.Configuration;
using BeatTogether.Status.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BeatTogether.Status.Api.Controllers
{
    [ApiController]
    [Route("info")]
    public class InfoController
    {
        private readonly InfoConfiguration _configuration;

        public InfoController(IOptionsSnapshot<InfoConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        [HttpGet]
        public MasterServerInfoData Get()
        {
            return new MasterServerInfoData(
                _configuration.ServerDisplayName,
                _configuration.ServerDescription,
                _configuration.ServerImageUrl,
                _configuration.MaxPlayers,
                _configuration.ServerSupportsPPModifiers,
                _configuration.ServerSupportsPPDifficulties,
                _configuration.ServerSupportsPPMaps);
        }
    }
}
