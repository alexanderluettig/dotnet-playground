
using Dotnet.Playground.Currencies;

namespace Dotnet.Playground.Stryker;

public class ExampleTests
{
    [Fact]
    public void It_should_convert_currency_correctly()
    {
        var euro = new Euro(10, 50);
        var cents = euro.ToCent();
        Assert.Equal(1050, cents);
    }
}