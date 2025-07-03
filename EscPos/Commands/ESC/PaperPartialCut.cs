using ReceiptPrinterEmulator.Emulator;
using ReceiptPrinterEmulator.Emulator.Enums;

namespace ReceiptPrinterEmulator.EscPos.Commands.ESC;


/// <summary>
/// 2024.02.18 Leo
/// Paper Movement Commands
/// https://escpos.readthedocs.io/en/latest/paper_movement.html#partial-cut-1b-69-rel-phx
/// </summary>
public class PaperPartialCut : BaseCommandNoArgs
{
    public override string Prefix => EscPosInterpreter.ESC + "i";

    public override void Execute(ReceiptPrinter printer, string? args)
    {
        var function = CutFunction.Cut;
        var shape = CutShape.Partial;

        printer.Cut(function, shape, 1);
    }
}