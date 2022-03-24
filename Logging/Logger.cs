using System;
using System.Text;

namespace ReceiptPrinterEmulator.Logging;

public static class Logger
{
    public static void Info(params object[] values)
    {
        PrintMessage("Info", values);
    }
    
    public static void Exception(Exception ex, string? message = null)
    {
        PrintMessage("Exception", new object[] { message ?? string.Empty, ex });
    }

    private static void PrintMessage(string prefix, object[] values)
    {
        var combinedMessage = $"[{prefix}] {FormatValues(values)}";
        Console.WriteLine(combinedMessage);
    }

    private static string FormatValues(params object[] values)
    {
        var sb = new StringBuilder();

        foreach (var val in values)
        {
            var asString = val.ToString();
            
            if (String.IsNullOrWhiteSpace(asString))
                continue;
            
            if (sb.Length > 0)
                sb.Append(' ');
            sb.Append(asString);
        }

        return sb.ToString();
    }
}