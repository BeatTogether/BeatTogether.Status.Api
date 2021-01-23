using System.Collections.Generic;

namespace BeatTogether.MasterServer.Api.Models
{
    public record UserMessage(List<LocalizedMessage> localizedMessages);
}
