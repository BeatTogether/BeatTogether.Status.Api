using BeatTogether.MasterServer.Interface.ApiInterface.Enums;
using BeatTogether.MasterServer.Interface.ApiInterface.Models;

namespace BeatTogether.Api.Models
{
    public record RegularServerTemplate(
        string ManagerId,
        GameplayServerConfiguration GameplayServerConfiguration,
        bool PermanantManager,
        float Timeout,
        string ServerName,
        BeatmapDifficultyMask BeatmapDifficultyMask,
        GameplayModifiersMask GameplayModifiersMask,
        SongPackMask SongPackMask);

}
