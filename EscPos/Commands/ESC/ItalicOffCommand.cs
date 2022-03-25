using ReceiptPrinterEmulator.Emulator;

namespace ReceiptPrinterEmulator.EscPos.Commands.ESC;

/// <summary>
/// Turn italics mode off
/// https://github.com/song940/node-escpos/blob/84149a67306857ad98cdafcd8384fb2b942e15da/packages/printer/commands.js
/// </summary>
public class ItalicOffCommand : BaseCommandNoArgs
{
    public override string Prefix => EscPosInterpreter.ESC + "5";

    public override void Execute(ReceiptPrinter printer, string? args)
    {
        printer.SelectItalicMode(false);
    }
}