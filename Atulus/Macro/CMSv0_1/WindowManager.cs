using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Atulus.Macro.CMSv0_1
{
    public class WindowManager
    {
        // Win32 API Imports
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// Waits for a window with the specified title to become active.
        /// </summary>
        /// <param name="windowTitle">The exact title of the window.</param>
        /// <param name="timeoutSeconds">Timeout in seconds. Use 0 to wait indefinitely.</param>
        /// <returns>True if the window became active; false if it timed out.</returns>
        public static bool WinWaitActive(string windowTitle, int timeoutSeconds = 0)
        {
            DateTime startTime = DateTime.Now;
            StringBuilder titleBuffer = new StringBuilder(256);

            while (true)
            {
                // 1. Get the handle of the currently active window
                IntPtr activeWindowHandle = GetForegroundWindow();

                if (activeWindowHandle != IntPtr.Zero)
                {
                    // 2. Get the text/title of that active window
                    GetWindowText(activeWindowHandle, titleBuffer, titleBuffer.Capacity);

                    // 3. Check if the active window title matches your target
                    if (titleBuffer.ToString().Equals(windowTitle, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }

                // 4. Check for timeout
                if (timeoutSeconds > 0 && (DateTime.Now - startTime).TotalSeconds >= timeoutSeconds)
                {
                    return false;
                }

                // 5. Sleep briefly to prevent high CPU usage (polling)
                Thread.Sleep(250);
            }
        }
    }
}
