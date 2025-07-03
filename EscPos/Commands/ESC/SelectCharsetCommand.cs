using ReceiptPrinterEmulator.Emulator;
using ReceiptPrinterEmulator.Emulator.Enums;

namespace ReceiptPrinterEmulator.EscPos.Commands.ESC;

/// <summary>
/// Select character font
/// https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=27
/// </summary>
public class SelectCharsetCommand : BaseCommand
{
    public override string Prefix => EscPosInterpreter.ESC + "R";
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
       
    }
}