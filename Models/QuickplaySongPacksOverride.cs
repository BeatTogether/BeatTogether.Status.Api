using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeatTogether.Status.Api.Models
{
    public record QuickplaySongPacksOverride(
        List<PredefinedPack> predefinedPackIds,
        List<LocalizedCustomPack> localizedCustomPacks);
}
