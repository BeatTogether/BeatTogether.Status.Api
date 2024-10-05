
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace BeatTogether.Status.Api.Controllers.Models
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public record UserMessage(List<LocalizedMessage> localizedMessages);
}
