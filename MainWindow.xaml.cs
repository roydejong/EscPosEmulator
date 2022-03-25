using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ReceiptPrinterEmulator.Emulator;
using ReceiptPrinterEmulator.Logging;
using ReceiptPrinterEmulator.Utils;
using Image = System.Windows.Controls.Image;

namespace ReceiptPrinterEmulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            App.Printer!.OnActivityEvent += (o, args) =>
            {
                RefreshUI();
                WindowsUtils.FlashWindow(this);
                WindowsUtils.ExclaimSoft();
            }; 
            
            RefreshUI();
        }

        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            Logger.Info("Resetting");
            
            App.Printer!.ReceiptStack.Clear();
            App.Printer.Initialize();
            App.Printer.StartNewReceipt();

            var toRemove = new List<Image>();
            foreach (var childControl in ReceiptImageRoot.Children)
                if (childControl is Image imgControl)
                    toRemove.Add(imgControl);
            toRemove.ForEach(img => ReceiptImageRoot.Children.Remove(img));

            RefreshUI();
        }

        private void TestButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("test_receipt.txt"))
                return;
            
            App.Printer?.FeedEscPos(File.ReadAllText("test_receipt.txt", Encoding.ASCII));
        }

        private void RefreshUI()
        {
            // Status label
            Address.Text = $"{App.Server!.EndPoint}";
            Address.Foreground = new SolidColorBrush(App.Server!.IsRunning ? Colors.SpringGreen : Colors.Crimson);

            // Receipt images
            foreach (var receipt in App.Printer!.ReceiptStack)
                CreateOrUpdateReceiptControl(ReceiptImageRoot, receipt);
            
            MainScrollView.ScrollToBottom();
        }

        private void CreateOrUpdateReceiptControl(Panel parentControl, Receipt receipt)
        {
            if (receipt.IsEmpty)
                return;
            
            var guidName = "R" + receipt.Guid.Replace("-", "");
            
            Image? ourControl = null;
            
            foreach (var childControl in parentControl.Children)
            {
                if (childControl is Image imgControl)
                {
                    if (imgControl.Name == guidName)
                    {
                        ourControl = imgControl;
                        break;
                    }
                }
            }

            if (ourControl == null)
            {
                ourControl = new Image();
                ourControl.Name = guidName;
                ourControl.Stretch = Stretch.None;
                ourControl.Margin = new Thickness(0, 0, 0, 10);
                
                parentControl.Children.Add(ourControl);
            }

            ourControl.Source = ConvertBitmap(receipt.Render());
        }
        
        /// <summary>
        /// Takes a bitmap and converts it to an image that can be handled by WPF ImageBrush
        /// </summary>
        /// <param name="src">A bitmap image</param>
        /// <returns>The image as a BitmapImage for WPF</returns>
        public BitmapImage ConvertBitmap(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
    }
}