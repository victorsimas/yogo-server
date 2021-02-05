using System.Collections.Generic;
using System.Threading.Tasks;
using YogoServer.Models;
using YogoServer.Requests;

namespace YogoServer.Services
{
    public interface IYogoService
    {
        Task<IEnumerable<string>> GetListEmails(InboxListRequest request);

        Task<Email> GetEmailMessage(InboxMailRequest request);
    }
}