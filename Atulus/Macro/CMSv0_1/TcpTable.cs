using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Atulus.Macro.CMSv0_1
{
    public class TcpTable
    {
        // Импорт функции Win32 API для получения таблицы сетевых соединений
        [DllImport("iphlpapi.dll", SetLastError = true)]
        private static extern uint GetExtendedTcpTable(IntPtr pTcpTable, ref int pdwSize, bool bOrder, int ulAf, TcpTableClass TableClass, uint Reserved = 0);

        private enum TcpTableClass
        {
            TCP_TABLE_OWNER_PID_ALL = 5
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MibTcprowOwnerPid
        {
            public uint state;
            public uint localAddr;
            public uint localPort;
            public uint remoteAddr;
            public uint remotePort;
            public uint owningPid;
        }


        [StructLayout(LayoutKind.Sequential)]
        private struct MibTcptableOwnerPid
        {
            public uint dwNumEntries;
            public MibTcprowOwnerPid table; // Фактически здесь массив строк
        }


        public static void Execute()
        {
            Console.WriteLine("Получение активных подключений...\n");
            Console.WriteLine($"{"Протокол",-10} {"Локальный адрес",-20} {"Внешний адрес",-20} {"PID",-10} {"Приложение"}");
            Console.WriteLine(new string('-', 85));

            // Получаем таблицу TCP
            int bufferSize = 0;
            GetExtendedTcpTable(IntPtr.Zero, ref bufferSize, true, 2, TcpTableClass.TCP_TABLE_OWNER_PID_ALL);
            IntPtr tcpTablePtr = Marshal.AllocHGlobal(bufferSize);

            try
            {
                uint result = GetExtendedTcpTable(tcpTablePtr, ref bufferSize, true, 2, TcpTableClass.TCP_TABLE_OWNER_PID_ALL);
                if (result == 0) // 0 означает NO_ERROR
                {
                    MibTcptableOwnerPid table = (MibTcptableOwnerPid)Marshal.PtrToStructure(tcpTablePtr, typeof(MibTcptableOwnerPid));
                    IntPtr rowPtr = (IntPtr)((long)tcpTablePtr + Marshal.OffsetOf(typeof(MibTcptableOwnerPid), "table").ToInt64());

                    int rowSize = Marshal.SizeOf(typeof(MibTcprowOwnerPid));

                    for (int i = 0; i < table.dwNumEntries; i++)
                    {
                        MibTcprowOwnerPid row = (MibTcprowOwnerPid)Marshal.PtrToStructure(rowPtr, typeof(MibTcprowOwnerPid));

                        // Переводим порты и адреса в читаемый формат
                        string localIp = new IPAddress(row.localAddr).ToString();
                        ushort localPort = BitConverter.ToUInt16(BitConverter.GetBytes(row.localPort), 0);
                        localPort = (ushort)IPAddress.NetworkToHostOrder((short)localPort);

                        string remoteIp = new IPAddress(row.remoteAddr).ToString();
                        ushort remotePort = BitConverter.ToUInt16(BitConverter.GetBytes(row.remotePort), 0);
                        remotePort = (ushort)IPAddress.NetworkToHostOrder((short)remotePort);

                        try
                        {
                            // Получаем процесс по его PID
                            Process process = Process.GetProcessById((int)row.owningPid);
                            string appName = process.ProcessName;
                            string appPath = "";

                            try { appPath = process.MainModule.FileName; }
                            catch { appPath = "Доступ ограничен"; }

                            Console.WriteLine($"{"TCP",-10} {localIp + ":" + localPort,-20} {remoteIp + ":" + remotePort,-20} {row.owningPid,-10} {appName} ({appPath})");
                        }
                        catch
                        {
                            // Если процесс завершился между вызовами или нет прав администратора
                            Console.WriteLine($"{"TCP",-10} {localIp + ":" + localPort,-20} {remoteIp + ":" + remotePort,-20} {row.owningPid,-10} Процесс завершен/Скрыт");
                        }

                        rowPtr = (IntPtr)((long)rowPtr + rowSize);
                    }
                }
            }
            finally
            {
                Marshal.FreeHGlobal(tcpTablePtr);
            }

        }
    }
}
