using ReceiptPrinterEmulator.Emulator;
using ReceiptPrinterEmulator.Emulator.Enums;

namespace ReceiptPrinterEmulator.EscPos.Commands.ESC;

/// <summary>
/// Select character font
/// https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=27
/// </summary>
public class SelectFontCommand : BaseCommand
{
    public override string Prefix => EscPosInterpreter.ESC + "M";
    public override bool HasArgs => true;
    
    private int _n;

    public override void Reset()
    {
        _n = 0;
    }
    
    public override bool InterpretNextChar(char c)
    {
        _n = c;
        return false;
    }

    public override void Execute(ReceiptPrinter printer, string? args)
    {
        switch (_n)
        {
            case 0 or 48:
                printer.SelectFont(PrinterFont.FontA);
                break;
            case 1 or 49:
                printer.SelectFont(PrinterFont.FontB);
                break;
            case 2 or 50:
                printer.SelectFont(PrinterFont.FontC);
                break;
            case 3 or 51:
                printer.SelectFont(PrinterFont.FontD);
                break;
            case 4 or 52:
                printer.SelectFont(PrinterFont.FontE);
                break;
            case 97:
                printer.SelectFont(PrinterFont.SpecialFontA);
                break;
            case 98:
                printer.SelectFont(PrinterFont.SpecialFontB);
                break;
        }
    }
}