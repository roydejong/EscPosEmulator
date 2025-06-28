using ReceiptPrinterEmulator.Emulator;

namespace ReceiptPrinterEmulator.EscPos.Commands.ESC;


/// <summary>
/// 2024.02.18 Leo
/// Paper Movement Commands
/// https://escpos.readthedocs.io/en/latest/paper_movement.html#print-and-feed-paper-1b-4a-phx
/// </summary>
public class PaperPrintFeed : BaseCommand
{
	public override string Prefix => EscPosInterpreter.ESC + "j";
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
		while (_n > 0) 
		{
			printer.LineFeed();
			_n--;
		}
	}
}
