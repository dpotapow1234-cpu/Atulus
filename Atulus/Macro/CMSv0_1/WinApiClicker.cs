using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Atulus.Macro.CMSv0_1
{
    public class WinApiClicker
    {
        // Импорт для отправки сообщений окну
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        // Константа сообщения нажатия кнопки
        private const uint BM_CLICK = 0x00F5;

        // Импорт для поиска кнопки по заголовку или классу (опционально)
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        //// hWndButton — это IntPtr с дескриптором (handle) нужной вам кнопки
        //SendMessage(hWndButton, BM_CLICK, IntPtr.Zero, IntPtr.Zero);

        // Find main window
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Post message asynchronously
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


        // WinAPI Constants
        const uint WM_CHAR = 0x0102;
        public static void SendTextViaPostMessage(IntPtr hWnd, string text)
        {
            foreach (char c in text)
            {
                // Send each character's scan code asynchronously
                PostMessage(hWnd, WM_CHAR, (IntPtr)c, IntPtr.Zero);

                // Short delay prevents the message queue from choking
                Thread.Sleep(10);
            }
        }

    }
}
