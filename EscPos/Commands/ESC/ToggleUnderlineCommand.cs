using ReceiptPrinterEmulator.Emulator;
using ReceiptPrinterEmulator.Emulator.Enums;

namespace ReceiptPrinterEmulator.EscPos.Commands.ESC;

/// <summary>
/// Turn underline mode on/off
/// https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=24
/// </summary>
public class ToggleUnderlineCommand : BaseCommand
{
    public override string Prefix => EscPosInterpreter.ESC + "-";
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
        if (_n is 0 or 48)
            printer.SelectUnderlineMode(UnderlineMode.Off);
        else if (_n is 1 or 49)
            printer.SelectUnderlineMode(UnderlineMode.OnOneDot);
        else if (_n is 2 or 50)
            printer.SelectUnderlineMode(UnderlineMode.OnTwoDots);
    }
}