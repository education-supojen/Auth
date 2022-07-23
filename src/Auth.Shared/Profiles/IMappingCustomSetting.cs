using AutoMapper;

namespace Auth.Shared.Profiles;

public interface IMappingCustomSetting
{
    void CreateMap(Profile profile);
}