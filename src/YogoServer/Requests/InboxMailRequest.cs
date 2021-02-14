using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YogoServer.Responses;

namespace YogoServer.Requests
{
    public class InboxMailRequest : InboxRequestBase
    {
        [Required(ErrorMessage = "It's Not specified the Index of the email")]
        public int Index { get; set; }

        [JsonIgnore]
        public override int AmountOrIndex 
        { 
            get
            {
                return Index;
            }
        }

        public bool Optmize { get; set; } = true;

        public async override Task<dynamic> DefineAsync(string yogoOutput)
        {
            Email email = new Email();

            string[] headAndBodySegment = yogoOutput.Split("---", StringSplitOptions.RemoveEmptyEntries);

            email.HeadEmail = Email.DefineMessageEmailHead(headAndBodySegment.FirstOrDefault());
            email.Body = Email.DefineMessageEmailBody(headAndBodySegment, Optmize);

            return await Task.FromResult(email);
        }
    }
}
