using System.Drawing;
using ReceiptPrinterEmulator.Emulator.Abstraction;

namespace ReceiptPrinterEmulator.Emulator.Printables;

public class ReceiptEmptyLine : IReceiptPrintable
{
    private readonly int _height;
    
    public ReceiptEmptyLine(int height)
    {
        _height = height;
    }

    public int GetPrintHeight() => _height;
    
    public void Render(Bitmap bitmap, Graphics g, int offsetX, int offsetY)
    {
    }
}