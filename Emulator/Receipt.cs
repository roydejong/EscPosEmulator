using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ReceiptPrinterEmulator.Emulator.Abstraction;

namespace ReceiptPrinterEmulator.Emulator;

public class Receipt
{
    private readonly PaperConfiguration _paperConfiguration;

    public readonly string Guid;
    
    private int PaperWidth => _paperConfiguration.GetPaperWidthInPixels();
    private int PrintWidth => _paperConfiguration.GetPrintWidthInPixels();
    private int PaperMargins => (PaperWidth - PrintWidth) / 2;

    private PrintMode _printMode;
    private List<IReceiptPrintable> _renderLines;
    private ReceiptLine? _currentTextLine;

    public bool IsEmpty => (_currentTextLine == null || _currentTextLine.IsEmpty) && _renderLines.Count == 0;

    public Receipt(PaperConfiguration paperConfiguration, PrintMode printMode)
    {
        Guid = System.Guid.NewGuid().ToString();
        
        _paperConfiguration = paperConfiguration;

        _printMode = printMode;
        _renderLines = new();
        _currentTextLine = null;
    }

    public void ChangeFontConfiguration(PrintMode printMode)
    {
        FinalizeTextLine();

        _printMode = printMode.Clone();
    }

    private ReceiptLine CreateNewTextLine() => new(_paperConfiguration, _printMode);
    
    public void PrintText(string text)
    {
        if (_currentTextLine is null)
            _currentTextLine = CreateNewTextLine();

        for (var i = 0; i < text.Length; i++)
        {
            var canContinue = _currentTextLine.TryWriteChar(text[i]);

            if (!canContinue)
            {
                FinalizeTextLine();

                _currentTextLine = CreateNewTextLine();
                canContinue = _currentTextLine.TryWriteChar(text[i]);

                if (!canContinue)
                    throw new Exception("Logic error - line must be able to contain > 0 chars");
            }
        }
    }

    public void FinalizeTextLine(bool forceNewLine = false)
    {
        if (_currentTextLine == null || _currentTextLine.IsEmpty)
        {
            if (forceNewLine)
            {
                _currentTextLine = CreateNewTextLine();
                _currentTextLine.TryWriteChar(' ');
                FinalizeTextLine(false);
            }
            return;
        }

        _renderLines.Add(_currentTextLine);
        _currentTextLine = null;
    }

    public void AdvanceToNewLine() => FinalizeTextLine(true);

    public int GetTotalPrintHeight()
        => _renderLines.Sum(line => line.GetPrintHeight());

    public int GetTotalPaperHeight() =>
        GetTotalPrintHeight() + (PaperMargins * 2);

    public Bitmap Render(bool drawPartials = true)
    {
        var paperWidth = PaperWidth;
        var paperHeight = GetTotalPaperHeight();
        
        var bmp = new Bitmap(paperWidth, paperHeight);
        using var g = Graphics.FromImage(bmp);
        
        // Fill white background
        g.FillRectangle(Brushes.White, 0, 0, paperWidth, paperHeight);
        
        // Draw all rendered lines
        var offsetX = PaperMargins;
        var offsetY = PaperMargins;

        foreach (var line in _renderLines)
        {
            line.Render(bmp, g, offsetX, offsetY);
            offsetY += line.GetPrintHeight();
        }

        return bmp;
    }
}