using BeatTogether.Status.Api.Enums;

namespace BeatTogether.Status.Api.Models
{
    public class LocalizedMessage 
    {
        public Language language { get; set; }
        public string message { get; set; } = string.Empty;
    }
}
