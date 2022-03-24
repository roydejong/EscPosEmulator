using ReceiptPrinterEmulator.Emulator;

namespace ReceiptPrinterEmulator.EscPos.Commands.GS;

/// <summary>
/// Select character size
/// https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=34
/// </summary>
public class SelectCharacterSizeCommand : BaseCommand
{
    public override string Prefix => EscPosInterpreter.GS + "!";
    public override bool HasArgs => true;
    
    private byte _n;

    public override void Reset()
    {
        _n = 0;
    }
    
    public override bool InterpretNextChar(char c)
    {
        _n = (byte)c;
        return false;
    }

    public override void Execute(ReceiptPrinter printer, string? args)
    {
        var widthMode = _n & 0b01110000; // bits 6,5,4
        var heightMode = _n & 0b00000111; // bits 2,1,0
        
        var charWidth = widthMode switch
        {
            0 => 1,
            16 => 2,
            32 => 3,
            48 => 4,
            64 => 5,
            80 => 6,
            96 => 7,
            112 => 8,
            _ => 1
        };

        var charHeight = heightMode switch
        {
            0 => 1,
            1 => 2,
            2 => 3,
            3 => 4,
            4 => 5,
            5 => 6,
            6 => 7,
            7 => 8,
            _ => 1
        };
        
        printer.SelectCharacterSize(charWidth, charHeight);
    }
}