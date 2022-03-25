using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ReceiptPrinterEmulator.Emulator;
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
            App.Printer!.OnActivityEvent += (o, args) => RefreshUI(); 
            
            RefreshUI();
        }

        private void RefreshUI()
        {
            Address.Text = $"Listening on TCP/IP ({App.Server!.EndPoint})";
            Address.Foreground = new SolidColorBrush(App.Server!.IsRunning ? Colors.SpringGreen : Colors.Crimson);

            // Receipt images
            CreateOrUpdateReceiptControl(ReceiptImageRoot, App.Printer!.CurrentReceipt);
            
            foreach (var receipt in App.Printer.ReceiptStack)
                CreateOrUpdateReceiptControl(ReceiptImageRoot, receipt);
        }

        private void CreateOrUpdateReceiptControl(Panel parentControl, Receipt receipt)
        {
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
                ourControl.Margin = new Thickness(0, 20, 0, 0);
                
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