using BeatTogether.Status.Api.Enums;

namespace BeatTogether.Status.Api.Models
{
    public class LocalizedCustomPackName
    {
        public Language language { get; set; }
        public string packName { get; set; } = string.Empty;
    }
}
