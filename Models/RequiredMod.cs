namespace BeatTogether.Status.Api.Models
{
    public record RequiredMod {
        public string id { get; set; } = string.Empty;
        public string version { get; set; } = string.Empty;
    }
}
