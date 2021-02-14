using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YogoServer.Responses;

namespace YogoServer.Requests
{
    public class InboxListRequest : InboxRequestBase
    {
        public int Amount { get; set; }

        public override int AmountOrIndex 
        { 
            get
            {
                return Amount;
            }
        }

        public async override Task<dynamic> DefineAsync(string yogoOutput)
        {
            Emails emails = new Emails() { Inbox = new List<string>() };
            
            string[] emailsArray = Regex.Split(yogoOutput, @"[ ]{1}[0-9]{1,3}[ ]{1}");

            foreach (string email in emailsArray)
            {
                if (!string.IsNullOrEmpty(email))
                {
                    emails.Inbox.Add(email);

                    int indx = emails.Inbox.FindIndex(indx => indx.Equals(email));
                    emails.Inbox[indx] =$"{indx + 1} {email.Replace("\n","")}";
                }
            }
            
            return await Task.FromResult(emails);
        }
    }
}
