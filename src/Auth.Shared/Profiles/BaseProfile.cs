using System.Collections.Immutable;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Shared.Profiles;

public class BaseProfile : Profile
{
    /// <summary>
    /// Extra:
    ///     轉成 ImmutableArray 的原因, IEnumerable 是不能更改內容的, ImmutableArray 才行
    /// </summary>
    public BaseProfile()
    {
        // 所有在建置時會被使用到的 Project 都會列入(But 要注意, 沒被建制的就不會樂入進來)
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        foreach (var assembly in assemblies)
        {
            var allTypes = assembly.GetTypes().Where(x => !x.IsAbstract && !x.IsInterface).ToList();
            var isCustomSettingSourceTypes = allTypes.Where(x => x.IsInterfaceTypeEqualToIMappingCustomSetting()).ToImmutableArray();
            var isMappingSourceTypes = allTypes.Where(t => t.IsInterfaceTypeEqualToIMapping()).ToImmutableArray();

            if (isCustomSettingSourceTypes != null) LoadICustomSetting(isCustomSettingSourceTypes);
            if (isMappingSourceTypes != null) LoadIMapping(isMappingSourceTypes);
        }
        
    }

    private void LoadICustomSetting(IEnumerable<Type> customSettingTypes)
    {
        foreach (var sourceType in customSettingTypes)
        {
            // Processing - 組建 Assembly 時,建立一個型別的 Instance
            var sourceModel = (IMappingCustomSetting)Activator.CreateInstance(sourceType)!;
            // Processing - 為 IMappingCustomSetting 物件生產 CreateMap Method
            sourceModel.CreateMap(this);
        }
    }
    
    /// <summary>
    /// 為實作 IMappingTo<T> & IMappingFrom<T> interface 的型別生產 CreateMap Method
    /// </summary>
    /// <param name="mappingTypes"></param>
    private void LoadIMapping(IEnumerable<Type> mappingTypes)
    {
        foreach (var sourceType in mappingTypes)
        {
            foreach (var interfaceType in sourceType.GetInterfaces())
            {
                // Processing - 如果型別跟 IMapping 沒關係就跳過
                if(!interfaceType.IsGenericType) continue;
                
                // Processing - 如果型別是 IMappingTo<> 就把鞋別映射出去
                if (interfaceType.GetGenericTypeDefinition() == typeof(IMappingTo<>))
                {
                    CreateMap(sourceType,interfaceType.GenericTypeArguments[0]);   
                }
                
                // Processing - 如果型別是 IMappingFrom<> 就把鞋別映射進來
                if (interfaceType.GetGenericTypeDefinition() == typeof(IMappingFrom<>))
                {
                    CreateMap(interfaceType.GenericTypeArguments[0],sourceType);   
                }
            }
        }
    }

}

/// <summary>
/// 一些為了取的型別特性的 extension
///     檢查型別有沒有
///         1. IMappingCustomSetting
///         2. IMappingTo<T>
///         3. IMappingFrom<T>
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    /// Description -
    ///     檢查類別是否有時做 IMappingTo<T> & IMappingFrom<T>
    ///
    /// Extra -
    ///     GetInterfaces()
    ///         取得類別的所有 interface
    ///     IsGenericType
    ///         檢查類別是否為範行類別
    ///     GetGenericTypeDefinition()
    ///         取得範行類別的類別資訊
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static bool IsInterfaceTypeEqualToIMapping(this Type t)
    {
        return t.GetInterfaces().Any(i => i.IsGenericType && 
                                          (i.GetGenericTypeDefinition() == typeof(IMappingTo<>) || 
                                           i.GetGenericTypeDefinition() == typeof(IMappingFrom<>)));
    }

    /// <summary>
    /// Description - 
    ///     檢查型別是否可以被指定給 IMappingCustomSetting interface
    /// Extra - 
    ///     Type.IsAssignableFrom(Type)
    ///         Determines whether an instance of a specified type c can be assigned to a variable of the current type.
    /// 給予另外一個型別
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static bool IsInterfaceTypeEqualToIMappingCustomSetting(this Type t)
    {
        return typeof(IMappingCustomSetting).IsAssignableFrom(t);
    }    
}

/// <summary>
/// Profile Dependency Injection
/// </summary>
public static class DependencyInjection 
{
    public static IServiceCollection AddProfiles(this IServiceCollection services)
    {
        // Processing - AutoMapper Profile 配置
        var mappingConfiguration = new MapperConfiguration(opt =>
        {
            opt.AddProfile(new BaseProfile());
        });
        var mapper = mappingConfiguration.CreateMapper();
        services.AddSingleton(mapper);

        // Output - 
        return services;
    }
}