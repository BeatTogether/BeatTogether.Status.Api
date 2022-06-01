using System.Collections.Generic;

namespace BeatTogether.Status.Api.Models
{
    public record UserMessage(List<LocalizedMessage> localizedMessages);
}
