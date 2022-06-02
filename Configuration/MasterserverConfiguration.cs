using BeatTogether.Api.Models;

namespace BeatTogether.Api.Configuration
{
    public class MasterserverConfiguration
    {
        public string Endpoint { get; set; } = "BeatTogether.SomeIPAddress";
        public string FullAccess { get; }  = "FULLACCESS"; //TODO Change this when put on main
        public string CreateAndDestryPermanentServers { get; } = "MIDACCESS"; //TODO change this when put on main
    }
}
