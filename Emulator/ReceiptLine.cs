using System.Drawing;
using ReceiptPrinterEmulator.Emulator.Abstraction;

namespace ReceiptPrinterEmulator.Emulator;

public class ReceiptLine : IReceiptPrintable
{
    private readonly PaperConfiguration.FontConfiguration _font;
    private readonly int _printWidth;
    private readonly int _charWidth;
    private readonly int _charHeight;

    private string _text;
    private int _totalWidth;

    public bool IsEmpty => string.IsNullOrEmpty(_text);
    
    public ReceiptLine(PaperConfiguration paperConfiguration, PrintMode printMode)
    {
        _font = paperConfiguration.GetFont(printMode.Font);
        _printWidth = paperConfiguration.GetPrintWidthInPixels();
        _charWidth = _font.CharacterWidth * printMode.CharWidthScale;
        _charHeight = _font.CharacterHeight * printMode.CharHeightScale;

        _text = "";
        _totalWidth = 0;
    }

    public bool TryWriteChar(char c)
    {
        if ((_totalWidth + _charWidth) >= _printWidth)
            return false;

        _text += c;
        _totalWidth += _charWidth;
        return true;
    }

    public int GetPrintHeight()
    {
        return _charHeight;
    }
    
    public void Render(Bitmap bitmap, Graphics g, int offsetX, int offsetY)
    {
        var font = new Font(_font.RenderFont, _charWidth);
        
        for (var i = 0; i < _text.Length; i++)
        {
            var c = _text[i];

            var rect = new Rectangle(
                x: (offsetX + (_charWidth * i)),
                y: offsetY,
                width: _charWidth,
                height: _charHeight
            );
            
            g.DrawString(c.ToString(), font, Brushes.Black, rect);
        }
    }
}