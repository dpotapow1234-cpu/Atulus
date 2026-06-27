using Atulus;
using Atulus.Macro.CMSv0_1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine(Atulus.Launchers.Direct.GetEpicGamesPath());

            var autoIt = new AutoItMacro(@"C:\AutoIt3\AutoIt3.exe");

            #region MacroV1

            Console.WriteLine("Waiting for Steam to become active...");
            // Waits up to 10 seconds for Steam
            bool success = WindowManager.WinWaitActive("Steam", 10);

            if (success)
            {
                Console.WriteLine("Steam is active! Proceeding with actions...");
            }
            else
            {
                Console.WriteLine("Timed out waiting for the window.");
            }

            var msg = MsgBox.Execute("Title", "Test", new uint[]{ MsgBox.MB_OK });
            // Handle the user response
            if (msg == MsgBox.IDOK)
            {
                Console.WriteLine("User selected Ok.");
            }

            #endregion

            #region AutoIt
            string script = "MsgBox(64, \"Test\", \"Hello from AutoIt\")" + "\r\n" +
                "Sleep(2000)";

            var result2 = await autoIt.RunScriptAsync(script);

            Console.WriteLine(result2.ExitCode);

            #endregion
        }
    }
}
