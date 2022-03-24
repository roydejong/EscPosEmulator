using System;

namespace ReceiptPrinterEmulator.EscPos;

public abstract class BaseCommandNoArgs : BaseCommand
{
    public override bool HasArgs => false;

    public override void Reset()
    {
    }

    public override bool InterpretNextChar(char c)
        => throw new InvalidOperationException("Command does not take args");
}