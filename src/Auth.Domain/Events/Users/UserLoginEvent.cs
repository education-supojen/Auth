using Auth.Domain.Aggregates;
using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Domain.Events.Users;

public record UserLoginEvent(User user) : IRequest<OneOf<bool, Failure>>;