// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Runtime.InteropServices;

namespace ParkitectNexus.Data
{
    internal class User32
    {
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}