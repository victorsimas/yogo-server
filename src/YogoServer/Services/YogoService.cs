using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YogoServer.Models;
using YogoServer.Requests;
using System.IO;
using Microsoft.Extensions.Logging;
using YogoServer.ErrorHandling;

namespace YogoServer.Services
{
    public class YogoService : IYogoService
    {
        private readonly Process _process;
        private readonly ILogger<IYogoService> _logger;

        public YogoService(Process process,ILogger<IYogoService> logger)
        {
            _process = process;
            _logger = logger;
        }

        public Task<IEnumerable<string>> GetListEmails(InboxListRequest request)
        {
            try
            {
                DefineProcess();

                _process.StartInfo.Arguments = $"inbox list {request.User} {request.Quantidade}";
                return Task.FromResult(DefineListEmail(ExecYogoProcess()));
            }
            catch(Exception ex)
            {
                throw new FailedDependencyException(ex.Message, new string[] { ex.Message } );
            }
        }

        public Task<Email> GetEmailMessage(InboxMailRequest request)
        {
            try
            {
                DefineProcess();

                _process.StartInfo.Arguments = $"inbox show {request.User} {request.Index}";
                return Task.FromResult(DefineMessageEmail(ExecYogoProcess(), request.Optmize, request.IgnoreRandomText));
            }
            catch(Exception ex)
            {
                throw new FailedDependencyException(ex.Message, new string[] { ex.Message } );
            }
        }

        private void DefineProcess()
        {
            
            _process.StartInfo.FileName = $"{AppDomain.CurrentDomain.BaseDirectory}yogoBinaries/yogo";
        }

        private string ExecYogoProcess()
        {
            _process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;
            _process.Start();

            string result = _process.StandardOutput.ReadToEnd();

            _process.WaitForExit();

            return result;
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
                HeadEmail = DefineMessageEmailHead(read.FirstOrDefault()),
                Body = DefineMessageEmailBody(read, optmize, ignoreRandomText)
            };
        }

        private HeadEmail DefineMessageEmailHead(string readFirstIndex)
        {
            if (readFirstIndex is not null)
            {
                string[] segment = readFirstIndex.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

                List<string> segmentGeneric = segment.OfType<string>().ToList();

                return new HeadEmail
                {
                    From = segmentGeneric?.FirstOrDefault(x => x.Contains("From", StringComparison.OrdinalIgnoreCase))?.Split(':')?.LastOrDefault(),
                    Title = segmentGeneric?.FirstOrDefault(x => x.Contains("Title", StringComparison.OrdinalIgnoreCase))?.Split(':')?.LastOrDefault(),
                    Date = segmentGeneric?.FirstOrDefault(x => x.Contains("Date", StringComparison.OrdinalIgnoreCase))?.Split(':')?.LastOrDefault(),
                };
            }
            
            return null;
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