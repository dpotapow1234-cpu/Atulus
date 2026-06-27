using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atulus;

namespace Utills
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            PowerShellRunner runner = new PowerShellRunner();
            var result = await runner.ExecuteAsync("Get-Process | Select-Object -First 5");

            Console.WriteLine("ExitCode: " + result.ExitCode);
            Console.WriteLine("STDOUT:");
            Console.WriteLine(result.StandardOutput);

            Console.WriteLine("STDERR:");
            Console.WriteLine(result.StandardError);

            Console.Write("Steam Launcher Path: ");
            Console.WriteLine(Atulus.Launchers.Direct.GetSteamPath());
            Console.Write("Epic Games Launcher Path: ");
            Console.WriteLine(Atulus.Launchers.Direct.GetSteamPath());

            var autoIt = new AutoItMacro(@"C:\AutoIt3\AutoIt3.exe");

            string script = "MsgBox(64, \"Test\", \"Hello from AutoIt\")" + "\r\n" +
                "Sleep(2000)";

            var result2 = await autoIt.RunScriptAsync(script);

            Console.WriteLine(result2.ExitCode);
        }
    }
}
