using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YogoServer.Models;
using YogoServer.Requests;

namespace YogoServer.Services
{
    public class YogoService : IYogoService
    {
        private readonly Process _process;

        public YogoService(Process process)
        {
            _process = process;
        }

        public Task<IEnumerable<string>> GetListEmails(InboxListRequest request)
        {
            try
            {
                _process.StartInfo.FileName = "yogo";
                _process.StartInfo.Arguments = $"inbox list {request.User} {request.Quantidade}";
            }
            catch(Exception)
            {
                return null;
            }

            return Task.FromResult(DefineListEmail(ExecYogoProcess()));
        }

        public Task<Email> GetEmailMessage(InboxMailRequest request)
        {
            try
            {
                _process.StartInfo.FileName = "yogo";
                _process.StartInfo.Arguments = $"inbox show {request.User} {request.Index}";
            }
            catch(Exception)
            {
                return null;
            }

            return Task.FromResult(DefineMessageEmail(ExecYogoProcess(), request.Optmize, request.IgnoreRandomText));
        }

        private string ExecYogoProcess()
        {
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.Start();

            _process.WaitForExit();

            return _process.StandardOutput.ReadToEnd();
        }

        private IEnumerable<string> DefineListEmail(string yogoOutput)
        {
            string[] read = yogoOutput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            return read;
        }

        private Email DefineMessageEmail(string yogoOutput, bool optmize, bool ignoreRandomText)
        {
            string[] read = yogoOutput.Split('-', StringSplitOptions.RemoveEmptyEntries);

            return new Email
            {
                HeadEmail = DefineMessageEmailHead(read[0]),
                Body = DefineMessageEmailBody(read, optmize, ignoreRandomText)
            };
        }

        private HeadEmail DefineMessageEmailHead(string readFirstIndex)
        {
            string[] segment = readFirstIndex.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            return new HeadEmail
            {
                From = segment[0].Split(':')[1],
                Title = segment[1].Split(':')[1],
                Date = segment[2].Split(':')[1]
            };
        }

        private Dictionary<string, List<string>> DefineMessageEmailBody(string[] read, bool optmize, bool ignoreRandomText)
        {
            Dictionary<string, List<string>> body = new Dictionary<string, List<string>>();

            for (int lineIndex = 1; lineIndex < read.Length; lineIndex++)
            {
                List<string> lineParts = new List<string>();
                
                foreach(var linePart in read[lineIndex].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (optmize)
                    {
                        var verifyRandomString = Regex.Match(linePart, @"[a-zA-Z0-9]{15,}");

                        if (verifyRandomString.Success)
                        {
                            continue;
                        }
                    }

                    lineParts.Add(linePart);
                }
                
                if (ignoreRandomText)
                {
                    if(lineParts.Count <= 1)
                    {
                        
                        continue;
                    }
                }

                body.Add($"lineMessageIndex{lineIndex}", lineParts);
            }

            return body;
        }
    }
}