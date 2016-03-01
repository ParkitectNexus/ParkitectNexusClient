// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Runtime.InteropServices;

// ReSharper disable All

namespace ParkitectNexus.Client.Windows
{
    internal class NativeMethods
    {
        public const int HWND_BROADCAST = 0xffff;
        public static readonly int WM_GIVEFOCUS = RegisterWindowMessage("WM_PN_FOCUS");

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SendNotifyMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
        
        [DllImport("user32.dll")]
        public static extern int RegisterWindowMessage(string message);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}