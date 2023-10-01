using Xunit;

namespace Tests.Services;

public class AccountServiceTests
{
    [Fact]
    public void Add_TwoNumbers_ReturnsSum()
    {
        var three = 1;
        var two = 2;

        var result = three + two;
        
        Assert.Equal(3, result);
    }
}