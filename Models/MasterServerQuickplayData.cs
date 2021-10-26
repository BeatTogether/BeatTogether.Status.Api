using System.Collections.Generic;

namespace BeatTogether.Status.Api.Models
{
    public record MasterServerQuickplayData(List<PredefinedPack> predefinedPackIds, List<LocalizedCustomPack> localizedCustomPacks);
}
