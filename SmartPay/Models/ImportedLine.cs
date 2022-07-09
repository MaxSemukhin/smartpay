namespace SmartPay.Models;

public class ImportedLine
{
    public int UserId { get; private set; }
    public int CheckId { get; private set; }
    public string ProcuctName { get; private set; }
    public int ProductCost { get; private set; }
    public string MerchantName { get; private set; }
    public int MCC { get; private set; }

    public ImportedLine(string[] line)
    {
        UserId = int.Parse(line[0]);
        CheckId = int.Parse(line[1]);
        ProcuctName = line[2];
        ProductCost = int.Parse(line[3]);
        MerchantName = line[4];
        MCC = int.Parse(line[5]);
    }
}