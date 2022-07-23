using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Aggregates;
using Auth.Domain.Enums;
using Auth.Domain.Factories.Interface;
using Auth.Domain.ValueObjects;

namespace Auth.Application.Factories;

public class UserFactory : IUserFactory
{
    private readonly IIdentityProducer _identityProducer;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IEmailMediator _emailMediator;

    public UserFactory(
        IIdentityProducer identityProducer,
        IPasswordHashService passwordHashService,
        IPasswordGenerator passwordGenerator,
        IEmailMediator emailMediator)
    {
        _identityProducer = identityProducer;
        _passwordHashService = passwordHashService;
        _passwordGenerator = passwordGenerator;
        _emailMediator = emailMediator;
    }
    
    /// <summary>
    /// 註冊新用戶
    /// </summary>
    /// <param name="name"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public User Create(string name, string email, Password password)
    {
        var id = _identityProducer.GetId();
        return new User(id, name, "人事主管", email, password);
    }

    /// <summary>
    /// 建立公司成員
    /// </summary>
    /// <param name="name"></param>
    /// <param name="email"></param>
    /// <param name="title"></param>
    /// <param name="boardingTime"></param>
    /// <returns></returns>
    public async Task<User> CreateAsync(string name, string email, string title, DateOnly boardingTime)
    {
        // Processing - 
        var id = _identityProducer.GetId();
        // Processing - 
        var password = _passwordGenerator.Generate();
        var hashPassword = _passwordHashService.Pbkdf2(password);
        await _emailMediator.SendEmployeePasswordAsync(email, name, password);
        // Processing - 
        return new User(id, name, email, hashPassword, boardingTime, title);
    }
}