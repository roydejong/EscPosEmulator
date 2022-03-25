using System;
using System.Collections.Generic;
using ReceiptPrinterEmulator.Emulator.Enums;

namespace ReceiptPrinterEmulator.Emulator;

public class PaperConfiguration
{
    private const double MmToInch = 0.0393701;

    public static PaperConfiguration Default => new();

    public double DotsPerInch = 180;
    public double PaperWidthMm = 80;
    public double PrintWidthMm = 72;
    public int DefaultLineSpacing = 10;

    public Dictionary<PrinterFont, FontConfiguration> _printerFonts = new()
    {
        {PrinterFont.FontA, new FontConfiguration(PrinterFont.FontA, 12, 24, "Consolas")},
        {PrinterFont.FontB, new FontConfiguration(PrinterFont.FontB, 12, 24, "Consolas")}
    };

    public FontConfiguration GetFont(PrinterFont printerFont)
    {
        if (_printerFonts.ContainsKey(printerFont))
            return _printerFonts[printerFont];

        if (printerFont != PrinterFont.FontA)
            return GetFont(PrinterFont.FontA);

        throw new InvalidOperationException($"Required font is missing from paper config: {printerFont}");
    }

    public double GetPaperWidthInInches() => PaperWidthMm * MmToInch;
    public double GetPrintWidthInInches() => PrintWidthMm * MmToInch;

    public int GetPaperWidthInPixels() => (int)Math.Ceiling(GetPaperWidthInInches() * DotsPerInch);
    public int GetPrintWidthInPixels() => (int)Math.Ceiling(GetPrintWidthInInches() * DotsPerInch);

    public class FontConfiguration
    {
        public PrinterFont PrinterFont;
        public int CharacterWidth;
        public int CharacterHeight;
        public string RenderFont;

        public FontConfiguration(PrinterFont printerFont, int characterWidth, int characterHeight, string renderFont)
        {
            PrinterFont = printerFont;
            CharacterWidth = characterWidth;
            CharacterHeight = characterHeight;
            RenderFont = renderFont;
        }
    }
}