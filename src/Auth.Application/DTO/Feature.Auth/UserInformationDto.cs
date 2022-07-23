using Auth.Domain.Enums;

namespace Auth.Application.DTO.Feature.Auth;

public class UserInformationDto
{
    /// <summary>
    /// 使用者 - ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 使用者 - 郵箱
    /// </summary>
    public string Email { get; set; }
}