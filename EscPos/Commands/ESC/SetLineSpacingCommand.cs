using ReceiptPrinterEmulator.Emulator;

namespace ReceiptPrinterEmulator.EscPos.Commands.ESC;

/// <summary>
/// Set line spacing
/// https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=20
/// </summary>
public class SetLineSpacingCommand : BaseCommand
{
    public override string Prefix => EscPosInterpreter.ESC + "3";
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
        printer.SetLineSpacing(_n);
    }
}