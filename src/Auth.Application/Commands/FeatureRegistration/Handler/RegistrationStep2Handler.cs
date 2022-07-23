using Auth.Domain.Enums;
using Auth.Domain.Errors;
using Auth.Domain.Repositories;
using MediatR;

namespace Auth.Application.Commands.FeatureRegistration.Handler;

public class RegistrationStep2Handler : IRequestHandler<RegistrationStep2Command>
{
    private readonly IRegistrationRepository _registrationRepository;

    public RegistrationStep2Handler(IRegistrationRepository registrationRepository)
    {
        _registrationRepository = registrationRepository;
    }
    
    public async Task<Unit> Handle(RegistrationStep2Command request, CancellationToken cancellationToken)
    {
        // Processing - Get the domain model
        var registration = await _registrationRepository.GetAsync(request.Email);
        if (registration == null) throw Errors.Registration.DoFirstStepFirst;
        
        // Processing - verify the code
        var verificationStatus = registration.VerifyCode(request.Code);

        // Processing - persist the domain model
        await _registrationRepository.UpdateAsync(registration);
        
        // Processing - check the code verification status
        switch (verificationStatus)
        {
            case VerificationStatus.Fail:
                throw Errors.Registration.VerificationUnCorrect;
            case VerificationStatus.HaveNoChance:
                throw Errors.Registration.TooManyTimes;
        }
        return Unit.Value;
    }
}