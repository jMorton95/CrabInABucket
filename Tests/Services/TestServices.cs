using Xunit;

namespace Tests.Services;

public class AccountServiceTests
{
    [Fact]
    public void Add_TwoNumbers_ReturnsSum()
    {
        var one = 1;
        var two = 2;

        var result = one + two;
        
        Assert.Equal(3, result);
    }
}