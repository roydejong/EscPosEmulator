using ReceiptPrinterEmulator.Emulator;

namespace ReceiptPrinterEmulator.EscPos.Commands.FS;


/// <summary>
/// 2024.02.18 Leo
/// Print a logo that is stored within the memory of your termal printer, n=1-255, m depends on printer model.
/// https://tabshop.smartlab.at/help-topics/help-esc-pos-codes.html
/// https://aures-support.com/DATA/drivers/Imprimantes/Commande%20ESCPOS.pdf
/// </summary>
public class PrintStoredLogo : BaseCommand
{
    public override string Prefix => EscPosInterpreter.FS + "p";
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
            return true;
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
    		// Normally we can't do anyting
    		// We can probably add a simulated logo in the future
    }
}