using BeatTogether.Status.Api.Enums;
using System.Collections.Generic;

namespace BeatTogether.Status.Api.Models
{
    public record MasterServerAvailabilityData(
        string minimumAppVersion,
        AvailabilityStatus status,
        long maintenanceStartTime,
        long maintenanceEndTime,
        UserMessage userMessage,
        List<RequiredMod> requiredMods,
        List<RequiredMod> blacklistMods);
}
