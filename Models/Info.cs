
namespace BeatTogether.Api.Models
{
    public record Info(
        string ServerDescription,
        string[] RequiredPCMods,
        string[] RequiredQUESTMods); //TODO this will most likely change
}
