using ReceiptPrinterEmulator.Emulator;

namespace ReceiptPrinterEmulator.EscPos.Commands.GS;


/// <summary>
/// Paper Movement Commands
/// https://escpos.readthedocs.io/en/latest/paper_movement.html#ejector-1d-65-rel
/// </summary>
public class PaperEjectCommand : BaseCommand
{
    public override string Prefix => EscPosInterpreter.GS + "E";
    public override bool HasArgs => true;
    
    private int _idx;
    private byte _n;
    private byte _m;
    private byte _t;

    public override void Reset()
    {
        _idx = 0;
        _n = _m = _t = 0;
    }
    
    public override bool InterpretNextChar(char c)
    {
        if (_idx == 0)
        {
            _idx++;
            _n = (byte)c;
            _m = 0;
            _t = 0;
            if (_n == 3 || _n == 32) return true;
        }
        else if (_idx == 1)
        {
            _idx++;
            _m = (byte)c;
            _t = 0;
            if (_n == 32) return true;
        }
        else if (_idx == 2)
        {
            _idx++;
            _t = (byte)c;
        }
        
        return false;
    }

    public override void Execute(ReceiptPrinter printer, string? args)
    {
        // Nothing to do
    }
}