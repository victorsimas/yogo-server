using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace YogoServer.Requests
{
    public abstract class InboxRequestBase<T>
    {
        [Required(ErrorMessage = "It's Not specified the user Yopmail")]
        public string User { get; set; }

        [BindNever]
        public abstract int AmountOrIndex { get; }

        public abstract Task<T> ExecuteAsync(string yogoOutput);
    }
}