namespace Auth.Presentation.Common;

/// <summary>
/// 令牌錯誤 - 重新登入
/// </summary>
public class TokenInvalidException : Exception { }

/// <summary>
/// 令牌過期 - 更新令牌
/// </summary>
public class TokenExpiredException : Exception { }