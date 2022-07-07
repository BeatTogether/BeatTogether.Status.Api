
using BeatTogether.Api.Models;

namespace BeatTogether.Api.Configuration
{
    public class MasterserverConfiguration
    {
        public int MinSecondsBetweenMSRequests { get; set; } = 8;
        public string Endpoint { get; set; } = "BeatTogether.SomeIPAddress"; //TODO set this automaticly when master is detected on rabbitmq, get master to broadcast an avent when it starts
        public string FullAccess { get; set; } = "FULLACCESS";
        public string CreateAndDestryPermanentServers { get; set; } = "MIDACCESS";
        public int MaxPlayersPerInstance { get; set; } = 250;
        public Info ModInfo { get; set; } = new("", System.Array.Empty<string>(), System.Array.Empty<string>()); //TODO change these values
        public string InGameMessage { get; set; } = "Pink cute"; //Idk, put something here and i think were gonna display it in game
    }
}
