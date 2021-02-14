using System.Threading.Tasks;
using YogoServer.Requests;

namespace YogoServer.Services
{
    public interface IYogoService
    {
        Task<dynamic> Get(InboxRequestBase request, string operation);
    }
}