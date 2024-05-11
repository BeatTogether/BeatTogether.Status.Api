using BeatTogether.Status.Api.Enums;
using System.Collections.Generic;

namespace BeatTogether.Status.Api.Models
{
    public record MasterServerStatusData(
        string minimumAppVersion,
        AvailabilityStatus status,
        long maintenanceStartTime,
        long maintenanceEndTime,
        UserMessage userMessage,
        List<RequiredMod> requiredMods,
        bool useSsl,
        string name,
        string description, 
        string imageUrl,
        int maxPlayers,
        bool supportsPpModifiers,
        bool supportsPpDifficulties,
        bool supportsPpMaps);
}
