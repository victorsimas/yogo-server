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

        public YogoService(Process process, ILogger<IYogoService> logger)
        {
            _process = process;
            _logger = logger;
        }

        public async Task<T> Get<T>(InboxRequestBase<T> request, string operation)
        {
            try
            {
                string sistemaOperacional = DefinirSistemaOperacional(Environment.OSVersion.Platform);

                _process.StartInfo.FileName = $"{AppDomain.CurrentDomain.BaseDirectory}yogoBinaries/yogo-{sistemaOperacional}";
                _process.StartInfo.Arguments = $"inbox {operation} {request.User} {request.AmountOrIndex}";

                return await request.ExecuteAsync(ExecYogoProcess());
            }
            catch(Exception ex)
            {
                throw new FailedDependencyException(ex.Message, new string[] { ex.Message } );
            }
        }

        private string DefinirSistemaOperacional(PlatformID platform) => (platform) switch
        {
            PlatformID.MacOSX => "macos",
            PlatformID.Unix => "unix",
            PlatformID.Win32NT => "windows.exe",
            _ => "unix"
        };

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