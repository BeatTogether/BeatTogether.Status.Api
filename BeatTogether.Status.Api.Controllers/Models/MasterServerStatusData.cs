using BeatTogether.Status.Api.Controllers.Enums;
using System.Collections.Generic;

namespace BeatTogether.Status.Api.Controllers.Models
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
