using System.Threading.Tasks;
using YogoServer.Requests;

namespace YogoServer.Services
{
    public interface IYogoService
    {
        Task<T> Get<T>(InboxRequestBase<T> request, string operation);
    }
}