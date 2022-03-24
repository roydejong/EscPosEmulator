using System;
using System.Text;
using ReceiptPrinterEmulator.Emulator;

namespace ReceiptPrinterEmulator.EscPos;

public class EscPosInterpreter
{
    private readonly ReceiptPrinter _printer;
    private readonly StringBuilder _printBuffer;
    
    public EscPosInterpreter(ReceiptPrinter printer)
    {
        _printer = printer;
        _printBuffer = new StringBuilder();
    }

    private string FinalizePrintBuffer()
    {
        var result = _printBuffer.ToString();
        _printBuffer.Clear();
        return result;
    }

    public void Interpret(string ascii)
    {
        for (var i = 0; i < ascii.Length; i++)
        {
            var currentChar = ascii[i];

            if (currentChar == HT)
            {
                // Horizontal tab
                throw new NotImplementedException("Not implemented: Horizontal tab");
            }
            
            if (currentChar == LF)
            {
                // Print and line feed
                _printer.PrintAndLineFeed(FinalizePrintBuffer());
                continue;
            }

            if (currentChar == FF)
            {
                // Print and return to Standard mode (in Page mode)
                throw new NotImplementedException("Not implemented: Print and return to Standard mode (in Page mode)");
            }

            if (currentChar == CR)
            {
                // Print and carriage return
                throw new NotImplementedException("Not implemented: Print and carriage return");
            }

            if (currentChar == DLE)
            {
                // Prefix for real-time commands (pulse, power-off, buzzer, status, etc)
                throw new NotImplementedException("Not implemented: DLE commands");
            }

            if (currentChar == CAN)
            {
                // Cancel print data in Page mode
                throw new NotImplementedException("Not implemented: Cancel print data in Page mode");
            }

            if (currentChar == ESC)
            {
                // ESC commands
                throw new NotImplementedException("Not implemented: ESC commands");
            }

            if (currentChar == FS)
            {
                // FS commands
                throw new NotImplementedException("Not implemented: FS commands");
            }

            if (currentChar == GS)
            {
                // GS commands
                throw new NotImplementedException("Not implemented: GS commands");
            }
            
            _printBuffer.Append(currentChar);
        }
    }
    
    public static readonly char NUL = Convert.ToChar(0);
    public static readonly char HT = Convert.ToChar(9);
    public static readonly char LF = Convert.ToChar(10);
    public static readonly char FF = Convert.ToChar(12);
    public static readonly char CR = Convert.ToChar(13);
    public static readonly char DLE = Convert.ToChar(16);
    public static readonly char CAN = Convert.ToChar(24);   
    public static readonly char ESC = Convert.ToChar(27);
    public static readonly char FS = Convert.ToChar(28);
    public static readonly char GS = Convert.ToChar(29);
}