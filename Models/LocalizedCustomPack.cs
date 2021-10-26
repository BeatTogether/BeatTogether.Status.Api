using System.Collections.Generic;

namespace BeatTogether.Status.Api.Models
{
    public record LocalizedCustomPack(string serializedName, int order, List<LocalizedCustomPackName> localizedNames, List<string> packIds);
}
