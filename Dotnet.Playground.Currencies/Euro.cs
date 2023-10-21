namespace Dotnet.Playground.Currencies;
public class Euro
{
    public Euro(int bigAmount, int smallAmount)
    {
        BigAmount = bigAmount;
        SmallAmount = smallAmount;
    }

    public int BigAmount { get; }
    public int SmallAmount { get; }

    public int ToCent() => BigAmount * 100 + SmallAmount;
}