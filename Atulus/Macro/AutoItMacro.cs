using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Atulus
{

    public class AutoItMacro
    {
        private readonly string _autoItExe;
        private Process _process;

        public AutoItMacro(string autoItExe)
        {
            _autoItExe = autoItExe;
        }

        public async Task<MacroResult> RunScriptAsync(string script)
        {
            string tempFile = Path.Combine(
                Path.GetTempPath(),
                $"{Guid.NewGuid()}.au3");

            File.WriteAllText(tempFile, script, Encoding.UTF8);

            try
            {
                return await RunFileAsync(tempFile);
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        public async Task<MacroResult> RunFileAsync(string scriptPath)
        {
            var stdout = new StringBuilder();
            var stderr = new StringBuilder();

            _process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _autoItExe,
                    Arguments = $"\"{scriptPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            _process.OutputDataReceived += (_, e) =>
            {
                if (e.Data != null)
                    stdout.AppendLine(e.Data);
            };

            _process.ErrorDataReceived += (_, e) =>
            {
                if (e.Data != null)
                    stderr.AppendLine(e.Data);
            };

            _process.Start();

            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            _process.WaitForExit();

            return new MacroResult
            {
                ExitCode = _process.ExitCode,
                Output = stdout.ToString(),
                Error = stderr.ToString()
            };
        }

        public void Stop()
        {
            if (_process is null || _process.HasExited == true) return;
            else _process.Kill();
        }
    }

    public class MacroResult
    {
        public int ExitCode { get; set; }
        public string Output { get; set; } = "";
        public string Error { get; set; } = "";
    }
}
