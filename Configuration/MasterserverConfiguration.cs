
namespace BeatTogether.Api.Configuration
{
    public class MasterserverConfiguration
    {
        public string Endpoint { get; set; } = "BeatTogether.SomeIPAddress";
        public string FullAccess { get; }  = "FULLACCESS"; //TODO Change this when put on main
        public string CreateAndDestryPermanentServers { get; } = "MIDACCESS"; //TODO change this when put on main
        //TODO make a better system for AccessTokens, eg loading a file into a data dictionary of access token to AllowedRequests, also save what access token made what server, so that only they can un-make the server
        //TODO cache values recieved from the master and dedi servers and only fetch from master or dedi if the last request was over 2 or 1 seconds ago to increase response timings if there are potentialy alot of users making many requests at once and reduce server lag.
    }
}
