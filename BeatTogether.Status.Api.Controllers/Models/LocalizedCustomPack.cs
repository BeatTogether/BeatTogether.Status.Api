using System.Collections.Generic;

namespace BeatTogether.Status.Api.Controllers.Models
{
    public class LocalizedCustomPack 
    {
        public string serializedName { get; set; } = string.Empty; 
        public int order { get; set; }
        public List<LocalizedCustomPackName> localizedNames { get; set; } = new();
        public List<string> packIds { get; set; } = new();
    }
}
