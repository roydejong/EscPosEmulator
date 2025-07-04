using ReceiptPrinterEmulator.Emulator.Abstraction;
using ReceiptPrinterEmulator.Logging;
using System;
using System.Drawing;

namespace ReceiptPrinterEmulator.Emulator.Printables;

public class ReceiptBitmapLine(PaperConfiguration paperConfiguration, Bitmap image) : IReceiptPrintable
{
    public int GetPrintHeight()
    {
        var printWidth = paperConfiguration.GetPrintWidthInPixels();
        if (image.Width <= printWidth)
            return image.Height;

        return (int)Math.Ceiling(image.Height * (float)printWidth / image.Width);
    }

    public void Render(Bitmap bitmap, Graphics g, int offsetX, int offsetY)
    {
        Logger.Info($"Rendering bitmap line at offset ({offsetX}, {offsetY}) with size ({image.Width}, {image.Height})");

        var printWidth = paperConfiguration.GetPrintWidthInPixels();
        if (image.Width <= printWidth)
        {
            // Center the image horizontally if it fits within the print width
            offsetX += (printWidth - image.Width) / 2;
            g.DrawImageUnscaled(image, offsetX, offsetY, image.Width, image.Height);
        }
        else
        {
            g.DrawImage(image, offsetX, offsetY, printWidth, image.Height * (float)printWidth / image.Width);
        }
    }
}
