using System.Threading.Tasks;
using WallBank.Application.Wrappers;

namespace WallBank.Application.Interfaces
{
    public interface IhttpClient
    {
        Task<(bool status, string message, object data)> PostData(object model, string url);
        Task<T> PostData<T>(object model, string url, string token);
        Task<(bool status, string message, object data)> PutData(object model, string url);

        Task<T> GetData<T>(string url);
        Task<object> GetDataObject(string url);

        Task<PagedResponse<IEnumerable<T>>> GetPaginatedData<T>(string url);

        void AddHeader(Dictionary<string, string> header);

    }
}
