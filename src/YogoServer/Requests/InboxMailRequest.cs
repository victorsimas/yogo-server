using System;
using System.Linq;
using System.Threading.Tasks;
using YogoServer.Responses;

namespace YogoServer.Requests
{
    public class InboxMailRequest : InboxRequestBase
    {
        public int Index { get; set; }

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
