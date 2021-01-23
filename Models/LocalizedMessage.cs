using BeatTogether.Status.Api.Enums;

namespace BeatTogether.Status.Api.Models
{
    public record LocalizedMessage(Language language, string message);
}
