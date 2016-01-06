// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Runtime.InteropServices;

namespace ParkitectNexus.Data.Game.Windows
{
    /// <summary>
    ///     Contains calls to the user32.dll API.
    /// </summary>
    internal class User32
    {
        /// <summary>
        ///     Brings the thread that created the specified window into the foreground and activates the window. Keyboard input is
        ///     directed to the window, and various visual cues are changed for the user. The system assigns a slightly higher
        ///     priority to the thread that created the foreground window than it does to other threads.
        /// </summary>
        /// <param name="hWnd">A handle to the window that should be activated and brought to the foreground.</param>
        /// <returns>true if the window was brought to the foreground; false if the window was not brought to the foreground.</returns>
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}