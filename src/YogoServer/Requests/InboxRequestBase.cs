using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace YogoServer.Requests
{
    public abstract class InboxRequestBase
    {
        [Required(ErrorMessage = "It's Not specified the user Yopmail")]
        public string User { get; set; }

        public abstract int AmountOrIndex { get; }

        public abstract Task<dynamic> DefineAsync(string yogoOutput);
    }
}