namespace ReceiptPrinterEmulator.Utils;

public static class ByteExtensions
{
    public static bool GetBit(this byte b, int bitNumber)
    {
        return (b & (1 << bitNumber)) != 0;
    }
}