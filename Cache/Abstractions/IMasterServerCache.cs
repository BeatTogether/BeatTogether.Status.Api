using BeatTogether.MasterServer.Interface.ApiInterface.Models;
using System.Threading.Tasks;

namespace BeatTogether.Api.Cache.Abstractions
{
    public interface IMasterServerCache
    {
        Task<SimpleServer?> GetPublicServerFromSecret(string secret);//
        Task<SimpleServer?> GetPublicServerFromCode(string code);//
        Task<SimpleServer?> GetServerFromSecret(string secret);//
        Task<SimpleServer?> GetServerFromCode(string code);//
        Task<string[]> GetAllSecrets();//
        Task<string[]> GetAllPublicSecrets();//
        Task<string[]> GetAllCodes();//
        Task<string[]> GetAllPublicCodes();//
        Task<SimpleServer[]> GetServers();//
        Task<SimpleServer[]> GetPublicServers();//
        Task<int> GetPlayerCount();//
        Task<int> GetPublicServerCount();//
        Task<int> GetAllServerCount();//
        Task<ServerNode[]> GetNodes();//
        Task<long> GetTotalJoins();//
    }
}
