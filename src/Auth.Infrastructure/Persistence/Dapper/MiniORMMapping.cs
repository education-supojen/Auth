using System.Reflection;
using Auth.Application.DTO;
using Auth.Application.DTO.Feature.Auth;
using Auth.Domain.Aggregates;
using Dapper;

namespace Auth.Infrastructure.Persistence.Dapper;

/// <summary>
/// Inject Mapping
/// </summary>
public static class InjectMiniORMMapping
{
    public static void Inject()
    {
        new UserInformationDtoMapping().Mapping();
    }
}

/// <summary>
/// Mapping Base Class
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MiniORMMappingBase<T>
{
    /// <summary>
    /// 什麼 property 要 mapping 到哪個 column
    /// </summary>
    public abstract Dictionary<string, string> columnMaps { get; }

    /// <summary>
    /// 進行 Mapping
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void Mapping()
    {
        var mapper = new Func<Type, string, PropertyInfo>((type, columnName) =>
            type.GetProperty(columnMaps.ContainsKey(columnName) ? columnMaps[columnName] : columnName));
        var mapping = new CustomPropertyTypeMap(typeof(T), (type, columnName) => mapper(type, columnName));
        SqlMapper.SetTypeMap(typeof(T), mapping);
    }
}

/// <summary>
/// UserInformationDto 的 Mapper
/// </summary>
public class UserInformationDtoMapping : MiniORMMappingBase<UserInformationDto>
{
    public override Dictionary<string, string> columnMaps => new()
    {
        {"CompanyId","company_id"}
    };
}
