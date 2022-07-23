using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Domain.Events.Registrations;

/// <summary>
/// 註冊 - 事件
/// </summary>
/// <param name="Email"></param>
/// <param name="Password"></param>
public record ReadyToRegisterUserEvent(string Email, string Password): IRequest<OneOf<bool,Failure>>;