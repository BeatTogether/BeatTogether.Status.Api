
using BeatTogether.DedicatedServer.Interface.Models;

namespace BeatTogether.Api.Models
{
    public record AdvancedServer(
        GameplayServerConfiguration GameplayServerConfiguration,
        string Secret,
        bool ReducedCpuMode,
        int PlayerCount, //If == 0, reduced cpu mode
        bool IsRunning,
        float RunTime,
        int Port,
        string ServerId,
        string ServerName,
        MultiplayerGameState MultiplayerGameState,
        GameplayManagerState GameplayManagerState,
        float NoPlayersTime,    //When RunTime-NoPlayersTime >= DestroyInstanceTimeout, it is stopped
        float DestroyInstanceTimeout, //-1 means permanent server
        bool PermanentServer,
        string SetManagerFromUserId, //If not blank then there is a permanent manager
        bool PermanentManager,
        float CountdownEndTime,     //CountdownEndTime-RunTime = countdown
        CountdownState CountdownState,
        GameplayModifiers SelectedGameplayModifiers,
        Beatmap SelectedBeatmap)
    {
        public static AdvancedServer Convert(AdvancedInstance instance, string secret)
        {
            bool reducedCpu = instance.PlayerCount == 0;
            bool PermanentServer = instance.DestroyInstanceTimeout == -1;
            bool PermanentManager = instance.SetManagerFromUserId != "";
            return new AdvancedServer(
                instance.GameplayServerConfiguration,
                secret,
                reducedCpu,
                instance.PlayerCount,
                instance.IsRunning,
                instance.RunTime,
                instance.Port,
                instance.UserId,
                instance.UserName,
                instance.MultiplayerGameState,
                instance.GameplayManagerState,
                instance.NoPlayersTime,
                instance.DestroyInstanceTimeout,
                PermanentServer,
                instance.SetManagerFromUserId,
                PermanentManager,
                instance.CountdownEndTime,
                instance.CountdownState,
                instance.SelectedGameplayModifiers,
                instance.SelectedBeatmap);
        }
    }

}
