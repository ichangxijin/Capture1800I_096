using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ImageCapturing
{
    public class WIN_MSG
    {
        private const int WM_USER = 0x4000;
        public const int WM_CAPTURE_DATA = WM_USER + 1;
        public const int WM_TRIGGER_CHANGE = WM_USER + 2;
        public const int WM_TRIGGER_STATUS = WM_USER + 3;
        public const int WM_SHOW_PROGRESS = WM_USER + 4;
        public const int WM_CAPTURE_WORKSTATUS = WM_USER + 5;
        public const int WM_LINKSTATUS = WM_USER + 6;
    }

    public unsafe class Kernel32Interface
    {
        public struct SECURITY_ATTRIBUTES
        {
            public uint nLength;
            public void* lpSecurityDescriptor;
            public int bInheritHandle;
        }

        [DllImport("Kernel32.dll")]
        public static extern IntPtr GetStdHandle(int mHandle);

        private const int STD_OUTPUT_HANDLE = -11;

        public static IntPtr GetStdIntptr()
        {
            return GetStdHandle(STD_OUTPUT_HANDLE);
        }

        [DllImport("Kernel32.dll")]
        public static extern IntPtr CreateEvent(SECURITY_ATTRIBUTES* lpEventAttributes, int bManualReset, int bInitialState, string lpName);

        [DllImport("Kernel32.dll")]
        public static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        [DllImport("Kernel32.dll")]
        public static extern int SetEvent(IntPtr hEvent);

        [DllImport("Kernel32.dll")]
        public static extern int CloseHandle(IntPtr hObject);

        [DllImport("Kernel32.dll")]
        public static extern int FlushConsoleInputBuffer(IntPtr hConsoleInput);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllname);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
        IntPtr hWnd, // handle to destination window
        int Msg, // message
        int wParam, // first message parameter
        int lParam // second message parameter
        );

        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
        IntPtr hWnd, // handle to destination window
        int Msg, // message
        int wParam, // first message parameter
        int lParam // second message parameter
        );
    }

    public unsafe class MemoryAlloc
    {
        const int HEAP_ZERO_MEMORY = 0x00000008;
        [DllImport("kernel32")]
        static extern int GetProcessHeap();

        [DllImport("kernel32")]
        static extern void* HeapAlloc(int hHeap,int flags, int size);

        [DllImport("kernel32")]
        static extern bool HeapFree(int hHeap, int flags, void* block);

        static int ph = GetProcessHeap();

        private MemoryAlloc(){}

        public static void* Alloc(int size)
        {
            void* result = HeapAlloc(ph, HEAP_ZERO_MEMORY, size);
            if (result == null)
            {
                throw new OutOfMemoryException();
            }
            return result;
        }

        public static void Free(void* block)
        {
            if (!HeapFree(ph,0,block))
            {
                throw new InvalidCastException();
            }
        }
    }
}