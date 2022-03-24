using ReceiptPrinterEmulator.Emulator;
using ReceiptPrinterEmulator.Emulator.Enums;

namespace ReceiptPrinterEmulator.EscPos.Commands.GS;

/// <summary>
/// Select cut mode and cut paper
/// https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=87
/// </summary>
public class SelectCutModeAndCutCommand : BaseCommand
{
    public override string Prefix => EscPosInterpreter.GS + "V";
    public override bool HasArgs => true;
    
    private int _idx;
    private int _m;
    private int _n;

    public override void Reset()
    {
        _idx = 0;
        _m = 0;
        _n = 0;
    }
    
    public override bool InterpretNextChar(char c)
    {
        if (_idx == 0)
        {
            _idx++;
            _m = c;
            _n = 0;

            // If "m" is greater than 49, it means cut function is B, C, or D with a second arg
            return (_m > 49);
        }
       
        if (_idx == 1)
        {
            _idx++;
            _n = c;
        }
        
        return false;
    }

    public override void Execute(ReceiptPrinter printer, string? args)
    {
        var function = CutFunction.Cut;
        var shape = CutShape.Full;
        
        switch (_n)
        {
            case 0 or 48:
                function = CutFunction.Cut;
                shape = CutShape.Full;
                break;
            case 1 or 49:
                function = CutFunction.Cut;
                shape = CutShape.Partial;
                break;
            case 65:
                function = CutFunction.FeedAndCut;
                shape = CutShape.Full;
                break;
            case 66:
                function = CutFunction.FeedAndCut;
                shape = CutShape.Partial;
                break;
            case 97:
                function = CutFunction.SetCutPos;
                shape = CutShape.Full;
                break;
            case 98:
                function = CutFunction.SetCutPos;
                shape = CutShape.Partial;
                break;
            case 103:
                function = CutFunction.FeedAndCutAndReverse;
                shape = CutShape.Full;
                break;
            case 104:
                function = CutFunction.FeedAndCutAndReverse;
                shape = CutShape.Partial;
                break;
        }
        
        printer.Cut(function, shape, _n);
    }
}