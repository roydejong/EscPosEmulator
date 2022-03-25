using System.Drawing;

namespace ReceiptPrinterEmulator.Emulator.Abstraction;

public interface IReceiptPrintable
{
    public void Render(Bitmap bitmap, Graphics g, int offsetX, int offsetY);
    public int GetPrintHeight();
}