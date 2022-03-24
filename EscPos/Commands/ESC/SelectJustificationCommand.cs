using ReceiptPrinterEmulator.Emulator;
using ReceiptPrinterEmulator.Emulator.Enums;

namespace ReceiptPrinterEmulator.EscPos.Commands.ESC;

/// <summary>
/// Select justification
/// https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=58
/// </summary>
public class SelectJustificationCommand : BaseCommand
{
    public override string Prefix => EscPosInterpreter.ESC + "a";
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
                printer.SelectJustification(TextJustification.Left);
                break;
            case 1 or 49:
                printer.SelectJustification(TextJustification.Center);
                break;
            case 2 or 50:
                printer.SelectJustification(TextJustification.Right);
                break;
        }
    }
}