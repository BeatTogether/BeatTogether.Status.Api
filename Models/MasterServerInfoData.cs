namespace BeatTogether.Status.Api.Models
{
    public record MasterServerInfoData(
        string name,
        string description, 
        string imageUrl,
        int maxPlayers,
        bool supportsPPModifiers,
        bool supportsPPDifficulties,
        bool serversupportsPPMaps);
}