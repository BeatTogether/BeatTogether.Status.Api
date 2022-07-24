using System.Collections.Generic;
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
        public List<RequiredMod> BlacklistMods { get; set; } = new();
        public List<string> AccessTokens { get; set; } = new() { "test2" };
    }
}
