using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Atulus.Macro.CMSv0_1
{
    public class Win32Input
    {
        // Импорт функции SendInput
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        // Структуры для событий (MOUSEINPUT, KEYBDINPUT, HARDWAREINPUT, INPUTUNION, INPUT)
        // Подробное описание структур доступно в документации
        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT { public uint type; public INPUTUNION u; }
        [StructLayout(LayoutKind.Explicit)]
        public struct INPUTUNION { [FieldOffset(0)] public KEYBDINPUT ki; /* ...мышь, hardware... */ }
        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT { public ushort wVk; public ushort wScan; public uint dwFlags; public uint time; public IntPtr dwExtraInfo; }

        // Константы
        public const uint INPUT_KEYBOARD = 1;
        public const uint KEYEVENTF_KEYUP = 0x0002;
        public const uint KEYEVENTF_UNICODE = 0x0004;

        // WinAPI Constants
        const uint WM_CHAR = 0x0102;


        // Метод для отправки текста
        public static void SendString(string text)
        {
            foreach (char c in text)
            {
                INPUT[] inputs = new INPUT[2];
                // Нажатие и отпускание (KEYEVENTF_UNICODE)
                inputs[0].type = inputs[1].type = INPUT_KEYBOARD;
                inputs[0].u.ki.wScan = inputs[1].u.ki.wScan = c;
                inputs[0].u.ki.dwFlags = KEYEVENTF_UNICODE;
                inputs[1].u.ki.dwFlags = KEYEVENTF_UNICODE | KEYEVENTF_KEYUP;
                SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
            }
        }
    }
}
