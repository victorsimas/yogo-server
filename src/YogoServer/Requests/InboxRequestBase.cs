using System.Threading.Tasks;

namespace YogoServer.Requests
{
    public abstract class InboxRequestBase
    {
        public string User { get; set; }

        public abstract int AmountOrIndex { get; }

        public abstract Task<dynamic> DefineAsync(string yogoOutput);
    }
}