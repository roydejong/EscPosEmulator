using System.Windows;
using ReceiptPrinterEmulator.Emulator;
using ReceiptPrinterEmulator.Networking;

namespace ReceiptPrinterEmulator
{
    public partial class App : Application
    {
        public static ReceiptPrinter? Printer = null;
        public static NetServer? Server = null;
        
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Printer = new ReceiptPrinter(PaperConfiguration.Default);

            Server = new NetServer(1234);
            _ = Server.Run();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Server?.Stop();
        }
    }
}