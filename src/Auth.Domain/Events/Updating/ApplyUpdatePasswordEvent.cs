using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Domain.Events.Updating;

/// <summary>
/// 申請更新密碼事件
/// </summary>
public record ApplyUpdatePasswordEvent(string Email, int Code) : IRequest<OneOf<bool,Failure>>;