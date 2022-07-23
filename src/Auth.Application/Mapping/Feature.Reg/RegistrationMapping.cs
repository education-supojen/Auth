using Auth.Application.Commands.Feature.Register;
using Auth.Domain.ValueObjects;
using Mapster;

namespace Auth.Application.Mapping.Feature.Reg;

public class RegistrationMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserRegistrationStep3Command, RegistrationInformation>();
    }
}