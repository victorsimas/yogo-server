using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using YogoServer.Responses;

namespace YogoServer.Requests
{
    public class InboxMailRequest : InboxRequestBase<Email>
    {
        [Required(ErrorMessage = "It's Not specified the Index of the email")]
        public int Index { get; set; }

        [BindNever]
        public override int AmountOrIndex
        { 
            get
            {
                return Index;
            }
        }

        public bool Optmize { get; set; } = true;

        public async override Task<Email> ExecuteAsync(string yogoOutput)
        {
            Email email = new Email();

            string[] headAndBodySegment = yogoOutput.Split("---", StringSplitOptions.RemoveEmptyEntries);

            email.HeadEmail = Email.DefineMessageEmailHead(headAndBodySegment.FirstOrDefault());
            email.Body = Email.DefineMessageEmailBody(headAndBodySegment, Optmize);

            return await Task.FromResult(email);
        }
    }
}
