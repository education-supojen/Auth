using Auth.Domain.Aggregates;
using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Domain.Events.Updating;

public record ReadyToUpdatePasswordEvent(User user, string password) : IRequest<OneOf<bool, Failure>>;