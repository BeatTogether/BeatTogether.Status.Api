using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace BeatTogether.Status.Api.Controllers.Models
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public record RequiredMod
    {
        /// <summary>
        /// BSIPA Mod ID
        /// </summary>
        public string id { get; set; } = string.Empty;
        /// <summary>
        /// Minimum version of the mod required, if installed
        /// </summary>
        public string version { get; set; } = string.Empty;
        /// <summary>
        /// Indicates whether the mod must be installed
        /// </summary>
        public bool required { get; set; } = false;
    }
}