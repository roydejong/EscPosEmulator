using System;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ReceiptPrinterEmulator.Utils;

public static class WindowsUtils
{
    [DllImport("user32")]
    public static extern int FlashWindow(IntPtr hwnd, bool bInvert);
    public static void FlashWindow(Window wnd) => FlashWindow(new WindowInteropHelper(wnd).Handle, true);
    
    public static void Exclaim() => SystemSounds.Exclamation.Play();
    public static void ExclaimSoft() => SystemSounds.Hand.Play();
}