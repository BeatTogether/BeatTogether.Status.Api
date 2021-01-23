using BeatTogether.MasterServer.Api.Enums;

namespace BeatTogether.MasterServer.Api.Models
{
    public record LocalizedMessage(Language language, string message);
}
