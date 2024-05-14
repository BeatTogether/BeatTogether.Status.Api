using BeatTogether.Status.Api.Controllers.Enums;

namespace BeatTogether.Status.Api.Controllers.Models
{
    public class LocalizedMessage 
    {
        public Language language { get; set; }
        public string message { get; set; } = string.Empty;
    }
}
