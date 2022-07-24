using BeatTogether.Status.Api.Enums;
using System.Collections.Generic;

namespace BeatTogether.Status.Api.Models
{
    public record MaintenanceTime(
        long maintenanceStartTime,
        long maintenanceEndTime);
}
