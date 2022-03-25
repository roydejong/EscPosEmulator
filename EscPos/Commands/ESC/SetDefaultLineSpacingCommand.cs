using ReceiptPrinterEmulator.Emulator;

namespace ReceiptPrinterEmulator.EscPos.Commands.ESC;

/// <summary>
/// Select default line spacing
/// https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=19
/// </summary>
public class SetDefaultLineSpacingCommand : BaseCommandNoArgs
{
    public override string Prefix => EscPosInterpreter.ESC + "2";
    
    public override void Execute(ReceiptPrinter printer, string? args)
    {
        printer.SetDefaultLineSpacing();
    }
}