﻿using BeatTogether.MasterServer.Interface.ApiInterface.Enums;
using BeatTogether.MasterServer.Interface.ApiInterface.Models;

namespace BeatTogether.Api.Models
{
    public record AdvancedServerTemplate(
        string ManagerId,
        GameplayServerConfiguration GameplayServerConfiguration,
        bool PermanentManager,
        float Timeout,
        bool PermanentServer,
        string ServerName,
        BeatmapDifficultyMask BeatmapDifficultyMask,
        GameplayModifiersMask GameplayModifiersMask,
        SongPackMask SongPackMask,
        string Code,
        string Secret);

}
