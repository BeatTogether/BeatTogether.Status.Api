
namespace BeatTogether.Api.Models
{
    public record MasterServerData(
        string Endpoint,
        int CurrentPublicInstanceCount,
        int CurrentInstancesCount);
}
