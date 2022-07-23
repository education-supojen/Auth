using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Auth;

public record UserLogoutCommand(string token) : IRequest<OneOf<bool,Failure>>;