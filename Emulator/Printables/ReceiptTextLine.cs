using System.Drawing;
using ReceiptPrinterEmulator.Emulator.Abstraction;
using ReceiptPrinterEmulator.Emulator.Enums;

namespace ReceiptPrinterEmulator.Emulator.Printables;

public class ReceiptTextLine : IReceiptPrintable
{
    private readonly PaperConfiguration.FontConfiguration _font;
    private readonly int _printWidth;
    private readonly int _charWidth;
    private readonly int _charHeight;
    private readonly TextJustification _justification;
    private readonly bool _bold;
    private readonly bool _italic;
    private readonly UnderlineMode _underline;

    private string _text;
    private int _totalWidth;

    public bool IsEmpty => string.IsNullOrEmpty(_text);
    
    public ReceiptTextLine(PaperConfiguration paperConfiguration, PrintMode printMode)
    {
        _font = paperConfiguration.GetFont(printMode.Font);
        _printWidth = paperConfiguration.GetPrintWidthInPixels();
        _charWidth = _font.CharacterWidth * printMode.CharWidthScale;
        _charHeight = _font.CharacterHeight * printMode.CharHeightScale;
        _justification = printMode.Justification;
        _bold = printMode.Emphasize;
        _italic = printMode.Italic;
        _underline = printMode.Underline;

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
        var fontStyle = FontStyle.Regular;
        if (_bold) fontStyle |= FontStyle.Bold;
        if (_italic) fontStyle |= FontStyle.Italic;
        
        var font = new Font(_font.RenderFont, _charWidth, fontStyle);
        
        for (var i = 0; i < _text.Length; i++)
        {
            var c = _text[i];

            var rect = new Rectangle(
                x: (offsetX + (_charWidth * i)),
                y: offsetY,
                width: _charWidth,
                height: _charHeight
            );

            if (_justification == TextJustification.Center)
            {
                rect.X += (_printWidth / 2) - (_totalWidth / 2);
            }
            else if (_justification == TextJustification.Right)
            {
                rect.X += (_printWidth - _totalWidth);
            }
            
            g.DrawString(c.ToString(), font, Brushes.Black, rect);

            if (_underline is UnderlineMode.OnOneDot or UnderlineMode.OnTwoDots)
            {
                var dotHeight = (_underline is UnderlineMode.OnTwoDots ? 2 : 1);
                g.DrawLine(new Pen(Color.Black, dotHeight), rect.Left, rect.Bottom, rect.Right, rect.Bottom);
            }
        }
    }
}