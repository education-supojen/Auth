using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Auth;

public record StaffLogoutCommand(string token) : IRequest<OneOf<bool,Failure>>;