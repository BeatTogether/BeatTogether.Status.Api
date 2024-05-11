namespace BeatTogether.Status.Api.Models
{
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