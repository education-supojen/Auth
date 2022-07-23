using Auth.Domain.Aggregates;
using Auth.Domain.Enums;
using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Domain.Events.Updating;

public record VerifyCodeForUpdatePasswordEvent(PasswordUpdate PasswordUpdate,VerificationStatus status) : IRequest<OneOf<bool,Failure>>;