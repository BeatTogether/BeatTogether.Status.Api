using BeatTogether.Status.Api.Enums;

namespace BeatTogether.Status.Api.Models
{
    public record MasterServerAvailabilityData(
        string minimumAppVersion,
        AvailabilityStatus status,
        long maintenanceStartTime,
        long maintenanceEndTime,
        UserMessage userMessage);
}
