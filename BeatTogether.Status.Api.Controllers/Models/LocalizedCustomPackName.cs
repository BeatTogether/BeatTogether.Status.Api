using BeatTogether.Status.Api.Controllers.Enums;

namespace BeatTogether.Status.Api.Controllers.Models
{
    public class LocalizedCustomPackName
    {
        public Language language { get; set; }
        public string packName { get; set; } = string.Empty;
    }
}
