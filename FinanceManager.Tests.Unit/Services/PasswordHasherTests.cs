using FinanceManager.Common.Services;
using Xunit;

namespace Tests.Services;

public class PasswordHasherTests
{
    private readonly PasswordUtilities _passwordUtilities;
    private readonly PasswordHasher _passwordHasher;
    
    public PasswordHasherTests()
    {
        _passwordUtilities = new PasswordUtilities();
        _passwordHasher = new PasswordHasher(_passwordUtilities);
    }

    public static List<object[]> PasswordsToHash =
    [
        ["password"],
        [""],
        ["                            "],
        ["thing"],
        ["12312381902381293"],
        ["83 24 73 897v3 498 07349803fv 4 8fc3 ydfguhdfghdf io"],
        ["admin123!"],
        ["Open@Sesame"],
        ["P@55w0rd!"],
        ["correcthorsebatterystaple"],
        ["letmein123"],
        ["password123!"],
        ["QwErTyUiOp"],
        ["123456"],
        ["!@#\\$%^&*()"],
        ["LongPassword!@#$%^&*1234567890"]
    ];
    
    [Theory, MemberData(nameof(PasswordsToHash))]
    public void TestPasswordHashing(string password)
    {
        var hashedPassword = _passwordHasher.HashPassword(password);
        
        Assert.True(_passwordHasher.CheckPassword(password, hashedPassword));
    } 
}