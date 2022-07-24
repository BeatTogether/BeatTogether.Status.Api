using System;
using System.Collections.Generic;
using BeatTogether.Status.Api.Configuration;
using BeatTogether.Status.Api.Enums;
using BeatTogether.Status.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BeatTogether.Status.Api.Controllers
{
    [ApiController]
    [Route("status")]
    public class StatusController : ControllerBase
    {
        private readonly StatusConfiguration _configuration;

        public StatusController(IOptionsMonitor<StatusConfiguration> configuration)
        {
            _configuration = configuration.CurrentValue;
        }

        [HttpGet]
        public MasterServerAvailabilityData Get()
        {
            var status = AvailabilityStatus.Online;
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (timestamp < _configuration.MaintenanceStartTime)
                status = AvailabilityStatus.MaintenanceUpcoming;
            else if (timestamp < _configuration.MaintenanceEndTime)
                status = AvailabilityStatus.Offline;
            return new MasterServerAvailabilityData(
                _configuration.MinimumAppVersion,
                status,
                _configuration.MaintenanceStartTime,
                _configuration.MaintenanceEndTime,
                new UserMessage(_configuration.LocalizedMessages),
                _configuration.RequiredMods,
                _configuration.BlacklistMods
            );
        }

        [HttpPost("{AccessToken}/SetRequiredMods/")]
        public IActionResult SetRequiredMods(string AccessToken, List<RequiredMod> RequiredMods)
        {
            if(!_configuration.AccessTokens.Contains(AccessToken))
                return Unauthorized();
            _configuration.RequiredMods = RequiredMods;
            return Accepted();
        }

        [HttpPost("{AccessToken}/SetBlacklistMods/")]
        public IActionResult SetBlacklistMods(string AccessToken, List<RequiredMod> BlacklistMods)
        {
            if (!_configuration.AccessTokens.Contains(AccessToken))
                return Unauthorized();
            _configuration.BlacklistMods = BlacklistMods;
            return Accepted();
        }

        [HttpPost("{AccessToken}/SetMaintenance/")]
        public IActionResult SetMaintenance(string AccessToken, MaintenanceTime maintenanceTime)
        {
            if (!_configuration.AccessTokens.Contains(AccessToken))
                return Unauthorized();
            if (maintenanceTime.maintenanceEndTime < maintenanceTime.maintenanceStartTime)
                return BadRequest();
            _configuration.MaintenanceStartTime = maintenanceTime.maintenanceStartTime;
            _configuration.MaintenanceEndTime = maintenanceTime.maintenanceEndTime;
            return Accepted();
        }
    }
}
