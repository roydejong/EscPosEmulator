using ReceiptPrinterEmulator.Emulator;
using ReceiptPrinterEmulator.Emulator.Enums;

namespace ReceiptPrinterEmulator.EscPos.Commands.ESC;

/// <summary>
/// 2024.02.18 Leo
/// Paper Movement Commands
/// https://escpos.readthedocs.io/en/latest/paper_movement.html#full-cut-1b-6d-phx
/// </summary>
public class PaperFullCut : BaseCommandNoArgs
{
    public override string Prefix => EscPosInterpreter.ESC + "m";

    public override void Execute(ReceiptPrinter printer, string? args)
    {
        var function = CutFunction.Cut;
        var shape = CutShape.Full;

        printer.Cut(function, shape, 0);
    }
}