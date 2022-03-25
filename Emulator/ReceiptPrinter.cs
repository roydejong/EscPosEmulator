using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ReceiptPrinterEmulator.Emulator.Enums;
using ReceiptPrinterEmulator.EscPos;
using ReceiptPrinterEmulator.Logging;

namespace ReceiptPrinterEmulator.Emulator;

public class ReceiptPrinter
{
    private readonly PaperConfiguration _paperConfiguration;
    private readonly EscPosInterpreter _escPosInterpreter;

    private PrintMode _printMode;
    
    public Receipt CurrentReceipt { get; private set; }
    public List<Receipt> ReceiptStack { get; private set; }

    public event EventHandler<EventArgs> OnActivityEvent;

    public ReceiptPrinter(PaperConfiguration paperConfiguration)
    {
        _paperConfiguration = paperConfiguration;
        _escPosInterpreter = new(this);

        _printMode = new PrintMode();

        ReceiptStack = new();

        StartNewReceipt();
        
        PowerCycle();
    }

    #region ESC/POS

    public void FeedEscPos(string ascii)
    {
        File.WriteAllText("last_escpos_receive.txt", ascii, Encoding.ASCII);

        try
        {
            _escPosInterpreter.Interpret(ascii);
        }
        catch (Exception ex)
        {
            Logger.Exception(ex, "ESC/POS Interpreter Error");
        }

        OnActivityEvent?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region Receipt meta

    private void StartNewReceipt()
    {
        CurrentReceipt = new(_paperConfiguration, _printMode);
        ReceiptStack.Add(CurrentReceipt);
    }

    #endregion

    #region Emulated

    public void PowerCycle()
    {
        Initialize();
    }

    #endregion

    #region Direct API

    public void Initialize()
    {
        SelectFont(PrinterFont.FontA);
        SelectJustification(TextJustification.Left);
        SelectCharacterSize(1, 1);
    }

    public void PrintText(string text)
    {
        Logger.Info($"Print: {text}");
        
        CurrentReceipt.PrintText(text);
    }

    public void Cut(CutFunction cutFunction = CutFunction.Cut, CutShape cutShape = CutShape.Full, int n = 0)
    {
        Logger.Info($"Execute cut: {cutFunction}, {cutShape}, {n}");
        
        if (cutFunction is CutFunction.FeedAndCut or CutFunction.FeedAndCutAndReverse)
            LineFeed();
        
        //StartNewReceipt(); // TODO Enable later
    }

    /// <summary>
    /// Feeds one line, based on the current line spacing.
    /// </summary>
    /// <remarks>
    /// - The amount of paper fed per line is based on the value set using the line spacing command (ESC 2 or ESC 3).
    /// </remarks>
    public void LineFeed()
    {
        Logger.Info($"Line feed");
        CurrentReceipt.AdvanceToNewLine();
    }

    public void SelectFont(PrinterFont printerFont)
    {
        Logger.Info($"Select font: {printerFont}");
        
        _printMode.Font = printerFont;
        CurrentReceipt.ChangeFontConfiguration(_printMode);
    }

    public void SelectJustification(TextJustification justification)
    {
        Logger.Info($"Select justification: {justification}");

        _printMode.Justification = justification;
        CurrentReceipt.ChangeFontConfiguration(_printMode);
    }

    public void SelectCharacterSize(int width, int height)
    {
        Logger.Info($"Set character size scale: x{width} width, x{height} height");

        _printMode.CharWidthScale = width;
        _printMode.CharHeightScale = height;
        CurrentReceipt.ChangeFontConfiguration(_printMode);
    }

    #endregion

    #region Command API

    /// <summary>
    /// Prints the data in the print buffer and feeds one line, based on the current line spacing.
    /// </summary>
    public void PrintAndLineFeed(string printBuffer)
    {
        PrintText(printBuffer);
        LineFeed();
    }

    #endregion
}