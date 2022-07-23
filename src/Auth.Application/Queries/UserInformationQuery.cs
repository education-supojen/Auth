using Auth.Application.DTO;
using MediatR;

namespace Auth.Application.Queries;

public record UserInformationQuery(string token) : IRequest<UserInformationDto>;