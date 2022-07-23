using Auth.Application.Interfaces.Services;
using Auth.Domain.Factories.Interface;
using Auth.Domain.Repositories;
using MediatR;

namespace Auth.Application.Commands.FeatureRegistration.Handler;

public class RegistrationStep1Handler : IRequestHandler<RegistrationStep1Command>
{
    private readonly IEmailMediator _emailMediator;
    private readonly IRegistrationFactory _registrationFactory;
    private readonly IRegistrationRepository _registrationRepository;
    

    public RegistrationStep1Handler(
        IEmailMediator emailMediator,
        IRegistrationFactory registrationFactory,
        IRegistrationRepository registrationRepository)
    {
        _emailMediator = emailMediator;
        _registrationFactory = registrationFactory;
        _registrationRepository = registrationRepository;
    }
    
    /// <summary>
    /// 註冊 - 第一步
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Unit> Handle(RegistrationStep1Command request, CancellationToken cancellationToken)
    {
        var registration = _registrationFactory.Create(request.Email);
        await _registrationRepository.AddAsync(registration);
        await _emailMediator.RegistrationWithEmailAsync(request.Email, registration.Code);
        return Unit.Value;
    }
}