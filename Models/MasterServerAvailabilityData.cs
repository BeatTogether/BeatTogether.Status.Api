using BeatTogether.MasterServer.Api.Enums;

namespace BeatTogether.MasterServer.Api.Models
{
    public record MasterServerAvailabilityData(
        string minimumAppVersion,
        AvailabilityStatus status,
        long maintenanceStartTime,
        long maintenanceEndTime,
        UserMessage userMessage);
}
