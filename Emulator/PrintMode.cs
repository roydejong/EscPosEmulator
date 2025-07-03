using ReceiptPrinterEmulator.Emulator.Enums;
using System;

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
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;
        if (obj is null || obj.GetType() != GetType())
            return false;

        var other = (PrintMode)obj;
        return Font == other.Font
            && CharWidthScale == other.CharWidthScale
            && CharHeightScale == other.CharHeightScale
            && Justification == other.Justification
            && Emphasize == other.Emphasize
            && Italic == other.Italic
            && Underline == other.Underline;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Font, CharWidthScale, CharHeightScale, Justification, Emphasize, Italic, Underline);
    }
}