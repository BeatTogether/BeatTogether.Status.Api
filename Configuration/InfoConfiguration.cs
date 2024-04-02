namespace BeatTogether.Status.Api.Configuration
{
    public class InfoConfiguration
    {
        public string ServerDisplayName { get; set; } = string.Empty;
        public string ServerDescription { get; set; } = string.Empty;
        public string ServerImageUrl { get; set; } = string.Empty;
        public int MaxPlayers { get; set; } = 25;
        public bool ServerSupportsPPModifiers { get; set; } = false;
        public bool ServerSupportsPPDifficulties { get; set; } = false;
        public bool ServerSupportsPPMaps { get; set; } = false;
    }
}
