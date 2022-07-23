using Auth.Application.DTO.Feature.Auth;
using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Queries.Feature.Auth;

public record UserInformationQuery(string token) : IRequest<OneOf<UserInformationDto,Failure>>;