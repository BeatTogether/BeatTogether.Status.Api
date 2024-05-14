using BeatTogether.Status.Api.Controllers.Models;
using System.Collections.Generic;

namespace BeatTogether.Status.Api.Controllers.Configuration
{
    public class QuickplayConfiguration
    {
        public List<PredefinedPack> PredefinedPacks { get; set; } = new();
        public List<LocalizedCustomPack> LocalizedCustomPacks { get; set; } = new();
    }
}
