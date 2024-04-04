﻿using System.Collections.Generic;
using BeatTogether.Status.Api.Models;

namespace BeatTogether.Status.Api.Configuration
{
    public class StatusConfiguration
    {
        public string MinimumAppVersion { get; set; } = "1.0.0";
        public long MaintenanceStartTime { get; set; }
        public long MaintenanceEndTime { get; set; }
        public List<LocalizedMessage> LocalizedMessages { get; set; } = new();
        public List<RequiredMod> RequiredMods { get; set; } = new();
        /// <summary>
        /// Prefer SSL (DTLS) for dedicated server connections? 
        /// </summary>
        public bool UseSsl { get; set; } = false;
        public string ServerDisplayName { get; set; } = string.Empty;
        public string ServerDescription { get; set; } = string.Empty;
        public string ServerImageUrl { get; set; } = string.Empty;
        public int MaxPlayers { get; set; } = 25;
        public bool ServerSupportsPPModifiers { get; set; } = false;
        public bool ServerSupportsPPDifficulties { get; set; } = false;
        public bool ServerSupportsPPMaps { get; set; } = false;
    }
}
