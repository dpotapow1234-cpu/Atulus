using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atulus.Launchers
{
    public class Steam
    {
        private Process _client;
        private string _steamPath;
        public bool isRun = false;
        
        public Steam()
        {
            _steamPath = Direct.GetSteamPath();
        }

        public async Task<ProcessResult> ExecuteAsync()
        {
            _client = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _steamPath,
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command ",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = false,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                }
            };

            var stdout = new StringBuilder();
            var stderr = new StringBuilder();

            _client.OutputDataReceived += (_, e) =>
            {
                if (e.Data != null)
                    stdout.AppendLine(e.Data);
            };

            _client.ErrorDataReceived += (_, e) =>
            {
                if (e.Data != null)
                    stderr.AppendLine(e.Data);
            };

            _client.Start();
            isRun = true;

            _client.BeginOutputReadLine();
            _client.BeginErrorReadLine();

            _client.WaitForExit();
            isRun = false;

            return new ProcessResult
            {
                ExitCode = _client.ExitCode,
                StandardOutput = stdout.ToString(),
                StandardError = stderr.ToString()
            };
        }
    }
}
