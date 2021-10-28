using System.Collections.Generic;

namespace BeatTogether.Status.Api.Models
{
    public class LocalizedCustomPack 
    {
        public string serializedName { get; set; } = ""; 
        public int order { get; set; }
        public List<LocalizedCustomPackName> localizedNames { get; set; } = new();
        public List<string> packIds { get; set; } = new();
    }
}
