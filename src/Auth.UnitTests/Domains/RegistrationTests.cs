using Auth.Application.Factories.Aggregates;
using Auth.Domain.Aggregates;
using Auth.Domain.Enums;
using Auth.Domain.Factories.Aggregates;
using Shouldly;
using Xunit;

namespace Auth.UnitTests.Domains;

public class RegistrationTests
{
    #region ARRANGE

    private readonly IRegistrationFactory _factory;

    public RegistrationTests() => _factory = new RegistrationFactory();

    private Registration GetRegistration()
    {
        // Processing - 
        var registration = _factory.Create("brian71742@gmail.com");
        // Processing - 
        registration.ClearEvents();
        // Processing - 
        return registration;
    }

    /// <summary>
    /// 取得一組驗證碼
    /// </summary>
    /// <param name="registration">註冊用資料</param>
    /// <param name="success">是否是正確驗證碼</param>
    /// <returns></returns>
    private int GetCode(Registration registration,bool success = false)
    {
        if (success)
        {
            return registration.Code;   
        }
        else
        {
            var random = new Random();
            var code = random.Next(100000, 999999);
            while (code == registration.Code) {
                code = random.Next(100000, 999999);
            }
            return code;    
        }
        
    }
    
    #endregion
    
    [Fact]
    public void VerifyCode_Success()
    {
        // Step1: Arrangement (準備資料)
        var registration = GetRegistration();
        var code = GetCode(registration, true);
        
        // Step2: Act (檢查邏輯)
        var result = registration.VerifyCode(code);

        // Step3: Assert (確認結果)
        result.ShouldBe(VerificationStatus.Success);
    }
    
    [Fact]
    public void VerifyCode_WrongCode()
    {
        // Step1: Arrangement 
        var registration = GetRegistration();

        // Description: 驗證3次, 結果分別要是 1.失敗 2.失敗 3.沒機會
        for (var iter = 1; iter <= 3; iter++)
        {
            // Arrange
            var code = GetCode(registration, false);
            
            // Act
            var result = registration.VerifyCode(code);
            
            //Assert
            if(iter != 3)
                result.ShouldBe(VerificationStatus.Fail);
            else
                result.ShouldBe(VerificationStatus.HaveNoChance);
        }
    }
}