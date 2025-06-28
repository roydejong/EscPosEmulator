using ReceiptPrinterEmulator.Emulator;

namespace ReceiptPrinterEmulator.EscPos.Commands.FS;


/// <summary>
/// 2024.02.18 Leo
/// Paper Movement Commands
/// https://escpos.readthedocs.io/en/latest/paper_movement.html#enable-and-disable-auto-cut-1c-7d-60-phx
/// </summary>
public class PaperAutoCut : BaseCommand
{
    public override string Prefix => EscPosInterpreter.ESC + "}";
    public override bool HasArgs => true;

    private int _idx;
    private byte _n;
    private byte _m;

    public override void Reset()
    {
        _idx = 0;
        _n = _m = 0;
    }
    
    public override bool InterpretNextChar(char c)
    {
        if (_idx == 0)
        {
            _idx++;
            _n = (byte)c;
            _m = 0;
            if (_n == 0x60) return true;
        }
        else if (_idx == 1)
        {
            _idx++;
            _m = (byte)c;
        }
        
        return false;
    }

    public override void Execute(ReceiptPrinter printer, string? args)
    {
        // Nothing to do
    }
}