using BeatTogether.Status.Api.Configuration;
using BeatTogether.Status.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BeatTogether.Status.Api.Controllers
{
    [ApiController]
    [Route("status/mp_override.json")]
    public class QuickplayController
    {
        private readonly QuickplayConfiguration _configuration;

        public QuickplayController(IOptionsSnapshot<QuickplayConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        [HttpGet]
        public MasterServerQuickplayData Get()
        {
            return new MasterServerQuickplayData(new QuickplaySongPacksOverride(
                _configuration.PredefinedPacks,
                _configuration.LocalizedCustomPacks
            ));
        }
    }
}
