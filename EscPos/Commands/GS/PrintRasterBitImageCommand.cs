using ReceiptPrinterEmulator.Emulator;
using ReceiptPrinterEmulator.Logging;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Input;

namespace ReceiptPrinterEmulator.EscPos.Commands.GS;

public class PrintRasterBitImageCommand : BaseCommand
{
    public override string Prefix => EscPosInterpreter.GS + "v0";
    public override bool HasArgs => true;

    private int n = 0;
    private int m = 0x00;
    private int xL = 0x00;
    private int xH = 0x00;
    private int width = 0;
    private int yL = 0x00;
    private int yH = 0x00;
    private int height = 0;
    private int length = 0;
    private byte[]? data = null;

    public override bool InterpretNextChar(char c)
    {
        switch (n++)
        {
            case 0:
                m = (int)c;
                return true;
            case 1:
                xL = (int)c;
                return true;
            case 2:
                xH = (int)c;
                width = (xH << 8) | xL;
                return true;
            case 3:
                yL = (int)c;
                return true;
            case 4:
                yH = (int)c;
                height = (yH << 8) | yL;
                length = width * height;
                width *= 8;
                data = new byte[length];
                return length > 0;
            default:
                data![n - 6] = (byte)c;
                return n - 5 < length;
        }
    }

    public override void Reset()
    {
        n = 0;
        m = 0x00;
        xL = 0x00;
        xH = 0x00;
        width = 0;
        yL = 0x00;
        yH = 0x00;
        height = 0;
        length = 0;
        data = null;
    }

    public override void Execute(ReceiptPrinter printer, string? args)
    {
        var bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        var values = ReadBytesByBits(length);

        BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);
        IntPtr ptr = bitmapData.Scan0;
        byte value = 0;
        for (int i = 0; i < values.Length; i++)
        {
            value = values[i] == 0 ? (byte)255 : (byte)0;
            System.Runtime.InteropServices.Marshal.WriteByte(ptr, i * 3 + 0, value);
            System.Runtime.InteropServices.Marshal.WriteByte(ptr, i * 3 + 1, value);
            System.Runtime.InteropServices.Marshal.WriteByte(ptr, i * 3 + 2, value);
        }

        bmp.UnlockBits(bitmapData);

        printer.PrintBitmap(bmp);
    }

    private byte[] ReadBytesByBits(int size)
    {
        byte[] result = new byte[size * 8];
        byte b;
        for (int i = 0; i < size; i++)
        {
            b = data![i];
            for (int j = 0; j < 8; j++)
            {
                result[i * 8 + j] = (byte)((b >> (7 - j)) & 1);
            }
        }
        return result;
    }
}
