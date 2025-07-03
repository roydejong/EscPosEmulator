using ReceiptPrinterEmulator.Emulator;
using ReceiptPrinterEmulator.Emulator.Enums;

namespace ReceiptPrinterEmulator.EscPos.Commands.ESC;

/// <summary>
/// 2024.02.18 Leo
/// Enable or disable print mode such as bold, italic or underline (n=00 off, n=10 double high, n=08 bold, n=40 italic).
/// https://tabshop.smartlab.at/help-topics/help-esc-pos-codes.html
/// https://aures-support.com/DATA/drivers/Imprimantes/Commande%20ESCPOS.pdf
/// </summary>
public class SetPrintTextMode : BaseCommand
{
    public override string Prefix => EscPosInterpreter.ESC + "!";
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
		if ((_n & 1) > 0) printer.SelectFont(PrinterFont.FontB);
        else printer.SelectFont(PrinterFont.FontA);
        
        // Bit 1 & 2 are unused
        
        if ((_n & 8) > 0) printer.SelectEmphasizeMode(true);
        else printer.SelectEmphasizeMode(false);
				
				if ((_n & 48) == 0) printer.SelectCharacterSize(1, 1); // Normal width & height
				else if ((_n & 48) == 16) printer.SelectCharacterSize(1, 2); // Double height 
				else if ((_n & 48) == 32) printer.SelectCharacterSize(2, 1); // Double width
				else printer.SelectCharacterSize(2, 2); // Double width & height 

        // Bit 6 is unused
				
				if ((_n & 128) > 0) printer.SelectUnderlineMode(UnderlineMode.OnOneDot);
				else printer.SelectUnderlineMode(UnderlineMode.Off);
    }
}