using Atulus;
using Atulus.Macro.CMSv0_1;
using AutoIt;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utills
{
    internal class Program
    {
        // Post message asynchronously
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        // WinAPI Constants
        const uint WM_CHAR = 0x0102;


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

            var steam = new Atulus.Launchers.Steam();
            Task.Factory.StartNew(() => steam.ExecuteAsync("runas -applaunch 730 /sandbox"), TaskCreationOptions.LongRunning);

            var autoIt = new AutoItMacro(@"C:\AutoIt3\AutoIt3.exe");

            #region AutoIt

            Console.WriteLine("Waiting for Steam to become active...");
            Console.WriteLine(AutoItX.WinActivate("Войти в Steam"));
            // Waits up to 10 seconds for Steam
            IntPtr steamHandle = Win32Native.FindWindow("Войти в Steam", null);
            bool success = WindowManager.WinWaitActive("Войти в Steam", 10);
            // 1. Find the top-level Notepad window

            if (success || steamHandle != IntPtr.Zero)
            {
                Console.WriteLine("Steam is active! Proceeding with actions...");
         

                Console.WriteLine(AutoItX.WinGetClassList("Войти в Steam"));
                Console.WriteLine(AutoItX.WinActivate("Войти в Steam"));
                Console.WriteLine(AutoItX.WinGetHandle("Войти в Steam"));
                WinApiClicker.SendTextViaPostMessage(AutoItX.WinGetHandle("Войти в Steam"), "123");

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
