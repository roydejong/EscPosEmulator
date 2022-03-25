using ReceiptPrinterEmulator.Emulator.Enums;

namespace ReceiptPrinterEmulator.Emulator;

public class PrintMode
{
    public PrinterFont Font;
    public int CharWidthScale;
    public int CharHeightScale;
    public TextJustification Justification;
    public bool Emphasize;
    public bool Italic;
    public UnderlineMode Underline;

    public PrintMode()
    {
        Initialize();
    }

    public PrintMode Clone()
    {
        return (PrintMode)MemberwiseClone();
    }
    
    public void Initialize()
    {
        Font = PrinterFont.FontA;
        CharWidthScale = 1;
        CharHeightScale = 1;
        Justification = TextJustification.Left;
    }
}