using System;
using System.Diagnostics;
using System.Threading.Tasks;
using YogoServer.Requests;
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

        public async Task<dynamic> Get(InboxRequestBase request, string operation)
        {
            try
            {
                _process.StartInfo.FileName = $"{AppDomain.CurrentDomain.BaseDirectory}yogoBinaries/yogo";
                _process.StartInfo.Arguments = $"inbox {operation} {request.User} {request.AmountOrIndex}";

                return await request.DefineAsync(ExecYogoProcess());
            }
            catch(Exception ex)
            {
                throw new FailedDependencyException(ex.Message, new string[] { ex.Message } );
            }
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
    }
}