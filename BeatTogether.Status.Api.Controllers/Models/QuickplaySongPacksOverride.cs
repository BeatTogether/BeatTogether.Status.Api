
namespace BeatTogether.Status.Api.Controllers.Models
{
    public record QuickplaySongPacksOverride(
        List<PredefinedPack> predefinedPackIds,
        List<LocalizedCustomPack> localizedCustomPacks);
}
