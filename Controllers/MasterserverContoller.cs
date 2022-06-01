using System;
using BeatTogether.Api.Configuration;
//using BeatTogether.Api.Enums;
using BeatTogether.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BeatTogether.Api.Controllers
{
    [ApiController]
    [Route("masterserver")]
    public class MasterserverController : ControllerBase
    {
        private readonly MasterserverConfiguration _configuration;

        public MasterserverController(IOptionsSnapshot<MasterserverConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        [HttpGet]
        public MasterServerData Get()
        {
            _configuration.CurrentInstanceCount = 0;
            _configuration.CurrentPublicInstanceCount = 0;
            return new MasterServerData(
                _configuration.CurrentInstanceCount,
                _configuration.CurrentPublicInstanceCount
            );
        }
    }
}
