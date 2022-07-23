using System.Security.AccessControl;
using Auth.Domain.Aggregates;
using Auth.Domain.Enums;
using MediatR;

namespace Auth.Application.Commands.FeatureBackstage;

public class AddEmployeeCommand : IRequest<User>
{
    /// <summary>
    /// 公司 ID
    /// </summary>
    public long CompanyId { get; set; }
    
    /// <summary>
    /// 使用者名稱
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 使用者郵箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 使用者職稱
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 使用者入職時間
    /// </summary>
    public DateOnly BoardingTime { get; set; }

    /// <summary>
    /// 權限
    /// </summary>
    public Permission Permission { get; set; }

    /// <summary>
    /// 部門 ID
    /// </summary>
    public long DepartmentId { get; set; }
}