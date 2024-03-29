﻿using System;
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

        public StatusController(IOptionsSnapshot<StatusConfiguration> configuration)
        {
            _configuration = configuration.Value;
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
                _configuration.RequiredMods
            );
        }
    }
}
