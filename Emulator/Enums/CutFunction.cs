namespace ReceiptPrinterEmulator.Emulator.Enums;

public enum CutFunction : byte
{
    /// <summary>
    /// Executes paper cut
    /// </summary>
    Cut,
    /// <summary>
    /// Feeds paper to [cutting position + (n × vertical motion unit)] and executes paper cut
    /// </summary>
    FeedAndCut,
    /// <summary>
    /// Preset [cutting position + (n × vertical motion unit)] to the paper cutting position, and executes paper cut when it reaches the autocutter position after printing and feeding
    /// </summary>
    SetCutPos,
    /// <summary>
    /// Feeds paper to [cutting position + (n × vertical motion unit)] and executes paper cut, then moves paper to the print start position by reverse feeding.
    /// </summary>
    FeedAndCutAndReverse
}