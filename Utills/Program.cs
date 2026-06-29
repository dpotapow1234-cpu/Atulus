using Atulus;
using Atulus.Macro.CMSv0_1;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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

            var steam = new Atulus.Launchers.Steam();
            await Task.Factory.StartNew(() => steam.ExecuteAsync("runas -applaunch 730"), TaskCreationOptions.LongRunning);

            var autoIt = new AutoItMacro(@"C:\AutoIt3\AutoIt3.exe");

            #region MacroV1

            Console.WriteLine("Waiting for Steam to become active...");
            // Waits up to 10 seconds for Steam
            bool success = WindowManager.WinWaitActive("Войти в Steam", 10);
            // 1. Find the top-level Notepad window
            IntPtr steamHandle = Win32Native.FindWindow("Войти в Steam", null);


            if (success || steamHandle != IntPtr.Zero)
            {
                Console.WriteLine("Steam is active! Proceeding with actions...");

                TcpTable.Execute();

                // 1. Указываем порт, который мы открыли в шаге 2
                ChromeOptions options = new ChromeOptions();
                options.DebuggerAddress = "127.0.0.1:51253";


                // 2. Подключаем Selenium к существующему браузеру
                IWebDriver driver = new ChromeDriver(options);

                try
                {
                    // Теперь можно работать с драйвером как обычно
                    Console.WriteLine("Успешно подключились к: " + driver.Title);

                    // Пример: переход на сайт
                    driver.Navigate().GoToUrl("https://www.google.com");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
                finally
                {
                    // НЕ вызывайте driver.Quit(), иначе браузер закроется. 
                    // Используйте driver.Close(), если хотите закрыть только текущую вкладку.

                    driver.Close();
                }
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
