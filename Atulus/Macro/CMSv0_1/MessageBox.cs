using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Atulus.Macro.CMSv0_1
{
    public class MsgBox
    {
        // 1. Import the native Windows API MessageBox function
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

        // 2. Define standard Win32 MessageBox constants
        // Button Configuration Flags
        public const uint MB_OK = 0x00000000;
        public const uint MB_OKCANCEL = 0x00000001;
        public const uint MB_ABORTRETRYIGNORE = 0x00000002;
        public const uint MB_YESNOCANCEL = 0x00000003;
        public const uint MB_YESNO = 0x00000004;

        // Icon Flags
        public const uint MB_ICONHAND = 0x00000010;          // Error Stop icon
        public const uint MB_ICONQUESTION = 0x00000020;      // Question icon
        public const uint MB_ICONEXCLAMATION = 0x00000030;  // Warning icon
        public const uint MB_ICONASTERISK = 0x00000040;     // Information icon

        // Return Value Constants
        public const int IDOK = 1;
        public const int IDCANCEL = 2;
        public const int IDABORT = 3;
        public const int IDRETRY = 4;
        public const int IDIGNORE = 5;
        public const int IDYES = 6;
        public const int IDNO = 7;

        public static int Execute(string Title, string Text, params uint[] flags)
        {
            // Combine flags using bitwise OR (|) to customize buttons and icons
            uint _flags = default;
            foreach(var flag in flags)
                _flags |= flag;

            // Call the Windows API
            return MessageBox(IntPtr.Zero, Text, Title, _flags);
        }
    }
}
