namespace Auth.Domain.Events;

/// <summary>
/// 事件 - 註冊
/// </summary>
/// <param name="email"></param>
/// <param name="code"></param>
public record RegistrationEvent(string email, int code): IDomainEvent;