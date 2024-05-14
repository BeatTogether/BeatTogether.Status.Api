using System;
using BeatTogether.Status.Api.Controllers.Configuration;
using BeatTogether.Status.Api.Controllers.Enums;
using BeatTogether.Status.Api.Controllers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BeatTogether.Status.Api.Controllers.Controllers
{
    [ApiController]
    [Route("status")]
    public class StatusController : ControllerBase
    {
        private readonly StatusConfiguration _configuration;

        public StatusController(IOptionsSnapshot<StatusConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        [HttpGet]
        public MasterServerStatusData Get()
        {
            var status = AvailabilityStatus.Online;
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (timestamp < _configuration.MaintenanceStartTime)
                status = AvailabilityStatus.MaintenanceUpcoming;
            else if (timestamp < _configuration.MaintenanceEndTime)
                status = AvailabilityStatus.Offline;
            return new MasterServerStatusData(
                _configuration.MinimumAppVersion,
                status,
                _configuration.MaintenanceStartTime,
                _configuration.MaintenanceEndTime,
                new UserMessage(_configuration.LocalizedMessages),
                _configuration.RequiredMods,
                _configuration.UseSsl,
                _configuration.ServerDisplayName,
                _configuration.ServerDescription,
                _configuration.ServerImageUrl,
                _configuration.MaxPlayers,
                _configuration.ServerSupportsPpModifiers,
                _configuration.ServerSupportsPpDifficulties,
                _configuration.ServerSupportsPpMaps
            );
        }
    }
}
