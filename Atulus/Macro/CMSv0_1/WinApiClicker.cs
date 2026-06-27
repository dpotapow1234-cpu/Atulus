using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Atulus.Macro.CMSv0_1
{
    class WinApiClicker
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

    }
}
