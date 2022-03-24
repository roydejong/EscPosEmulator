using ReceiptPrinterEmulator.Emulator;

namespace ReceiptPrinterEmulator.EscPos;

public abstract class BaseCommand
{
    /// <summary>
    /// Gets the full command prefix as string, e.g. "[ESC]M" for the "Select font" command
    /// </summary>
    public abstract string Prefix { get; }
    /// <summary>
    /// Gets whether this command takes args.
    /// </summary>
    public abstract bool HasArgs { get; }

    /// <summary>
    /// Called before beginning arg interpreting.
    /// </summary>
    public abstract void Reset();
    
    /// <summary>
    /// If this command has args, this method will be called for each character following the prefix.
    /// Characters will continue to be interpreted as args until this method returns false.
    /// </summary>
    /// <param name="c">The character to be interpreted</param>
    /// <returns>TRUE to continue interpreting args, FALSE to return to normal mode</returns>
    public abstract bool InterpretNextChar(char c);

    /// <summary>
    /// Executes the command, once all args have been interpreted.
    /// </summary>
    /// <param name="args">The combined args (everything past the command prefix seen by InterpretNextChar)</param>
    public abstract void Execute(ReceiptPrinter printer, string? args);
}