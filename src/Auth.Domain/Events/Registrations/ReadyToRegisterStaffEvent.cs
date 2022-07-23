using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Domain.Events.Registrations;

public record ReadyToRegisterStaffEvent(string Email, string Password): IRequest<OneOf<bool,Failure>>;
