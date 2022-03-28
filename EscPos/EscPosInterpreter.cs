using System;
using System.Collections.Generic;
using System.Text;
using ReceiptPrinterEmulator.Emulator;
using ReceiptPrinterEmulator.EscPos.Commands.ESC;
using ReceiptPrinterEmulator.EscPos.Commands.GS;
using ReceiptPrinterEmulator.Logging;

namespace ReceiptPrinterEmulator.EscPos;

public class EscPosInterpreter
{
    private readonly ReceiptPrinter _printer;
    private readonly StringBuilder _printBuffer;
    private readonly StringBuilder _commandBuffer;
    private readonly Dictionary<string, BaseCommand> _commandRegistry;

    private int _maxCommandPrefixLength;

    private bool _interpretingCommandPrefix;
    private bool _interpretingCommandArgs;
    private BaseCommand? _activeCommand;

    public EscPosInterpreter(ReceiptPrinter printer)
    {
        _printer = printer;
        _printBuffer = new StringBuilder();
        _commandBuffer = new StringBuilder();
        _commandRegistry = new();

        _maxCommandPrefixLength = 0;

        _interpretingCommandPrefix = false;
        _interpretingCommandArgs = false;
        _activeCommand = null;

        RegisterCommands();
    }

    #region Command registry

    private void RegisterCommands()
    {
        // ESC
        RegisterCommand(new InitializePrinterCommand());
        RegisterCommand(new ItalicOffCommand());
        RegisterCommand(new ItalicOnCommand());
        RegisterCommand(new SelectFontCommand());
        RegisterCommand(new SelectJustificationCommand());
        RegisterCommand(new SetDefaultLineSpacingCommand());
        RegisterCommand(new SetLineSpacingCommand());
        RegisterCommand(new ToggleEmphasizeCommand());
        RegisterCommand(new ToggleUnderlineCommand());
        
        // GS
        RegisterCommand(new SelectCharacterSizeCommand());
        RegisterCommand(new SelectCutModeAndCutCommand());
    }

    private void RegisterCommand(BaseCommand command)
    {
        var prefix = command.Prefix;

        if (_commandRegistry.ContainsKey(prefix))
            throw new ArgumentException($"Cannot register command with duplicate prefix: {prefix}");

        _commandRegistry.Add(prefix, command);

        if (prefix.Length > _maxCommandPrefixLength)
            _maxCommandPrefixLength = prefix.Length;
    }

    #endregion

    #region Buffers

    public void ClearBuffers()
    {
        FinalizePrintBuffer();
        FinalizeCommandBuffer();
    }
    
    private string FinalizePrintBuffer()
    {
        var result = _printBuffer.ToString();
        _printBuffer.Clear();
        return result;
    }

    private string FinalizeCommandBuffer()
    {
        var result = _commandBuffer.ToString();
        _commandBuffer.Clear();
        return result;
    }

    #endregion

    public void Interpret(string ascii)
    {
        for (var i = 0; i < ascii.Length; i++)
        {
            var currentChar = ascii[i];

            #region Command modes

            if (_interpretingCommandArgs)
            {
                // Reading command args: keep reading until the command is done interpreting
                _commandBuffer.Append(currentChar);

                var shouldContinue = _activeCommand!.InterpretNextChar(currentChar);

                if (!shouldContinue)
                {
                    var finalArgs = FinalizeCommandBuffer();

                    Logger.Info($"Execute [{_activeCommand.GetType().Name}] with args [{finalArgs}]");

                    _activeCommand.Execute(_printer, finalArgs);
                    _activeCommand = null;

                    _interpretingCommandPrefix = false;
                    _interpretingCommandArgs = false;
                }

                continue;
            }

            if (_interpretingCommandPrefix)
            {
                // Reading command prefix: keep reading until we find a match or hit _maxCommandPrefixLength
                _commandBuffer.Append(currentChar);

                var commandText = _commandBuffer.ToString();

                if (commandText.Length > _maxCommandPrefixLength)
                    throw new InvalidOperationException($"Invalid or unsupported command encountered: {commandText}");

                if (_commandRegistry.ContainsKey(commandText))
                {
                    // Found matching registered command
                    _activeCommand = _commandRegistry[commandText];
                    _activeCommand.Reset();
                    
                    _commandBuffer.Clear();

                    if (_activeCommand.HasArgs)
                    {
                        // This command has arguments: begin interpreting those
                        _interpretingCommandPrefix = false;
                        _interpretingCommandArgs = true;
                    }
                    else
                    {
                        // This command has NO arguments: execute immediately and return to normal mode
                        _interpretingCommandPrefix = false;
                        _interpretingCommandArgs = false;

                        Logger.Info($"Execute [{_activeCommand.GetType().Name}]");

                        _activeCommand.Execute(_printer, null);
                        _activeCommand = null;
                    }
                }

                continue;
            }

            #endregion

            #region Normal mode

            if (currentChar == HT)
            {
                // Horizontal tab
                throw new NotImplementedException("Not implemented: Horizontal tab");
            }

            if (currentChar == LF || currentChar == CR)
            {
                // Print and line feed
                _printer.PrintAndLineFeed(FinalizePrintBuffer());
                continue;
            }

            if (currentChar == FF)
            {
                // Print and return to Standard mode (in Page mode)
                throw new NotImplementedException("Not supported: page mode");
            }

            if (currentChar == DLE)
            {
                // Prefix for real-time commands (pulse, power-off, buzzer, status, etc)
                throw new NotImplementedException("Not supported: DLE / real time commands");
            }

            if (currentChar == CAN)
            {
                // Cancel print data in Page mode
                throw new NotImplementedException("Not supported: page mode");
            }

            if (currentChar == ESC || currentChar == FS || currentChar == GS)
            {
                // ESC, FS and GS commands - begin command mode
                _interpretingCommandPrefix = true;

                _commandBuffer.Clear();
                _commandBuffer.Append(currentChar);
                continue;
            }

            if (currentChar == NUL)
            {
                // Null byte outside of command context; do nothing
                continue;
            }

            // Regular character, not in command mode: append to print buffer
            _printBuffer.Append(currentChar);

            #endregion
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