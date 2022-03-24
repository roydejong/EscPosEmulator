using System.IO;
using System.Text;
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
            Printer = new ReceiptPrinter();

            Server = new NetServer(1234);
            _ = Server.Run();
            
            if (File.Exists("last_escpos_receive.txt"))
                Printer.FeedEscPos(File.ReadAllText("last_escpos_receive.txt", Encoding.ASCII));
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Server?.Stop();
        }
    }
}