using ReceiptPrinterEmulator.Emulator;

namespace ReceiptPrinterEmulator.EscPos.Commands.ESC;

/// <summary>
/// Turn emphasized mode on/off
/// https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=25
/// </summary>
public class ToggleEmphasizeCommand : BaseCommand
{
    public override string Prefix => EscPosInterpreter.ESC + "E";
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
        if (_n is 0)
            printer.SelectEmphasizeMode(false);
        else if (_n is 1)
            printer.SelectEmphasizeMode(true);
    }
}