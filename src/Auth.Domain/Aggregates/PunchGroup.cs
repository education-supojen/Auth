using Auth.Domain.ValueObjects;

namespace Auth.Domain.Aggregates;

public class PunchGroup : IEntityBase<long>
{
    public PunchGroup() { }
    
    public PunchGroup(string name,Location location, float validDistance)
    {
        Name = name;
        Latitude = location.latitude;
        Longitude = location.longitude;
        Address = location.address;
        ValidDistance = validDistance;
    }
    
    /// <summary>
    /// 打卡設定 ID
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// 打卡設定名稱
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 緯度
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// 經度
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// 打卡有效距離
    /// </summary>
    public float ValidDistance { get; set; }
    
    /// <summary>
    /// 屬於哪家公司
    /// </summary>
    public virtual Company Company { get; set; }
    
    /// <summary>
    /// 使用者
    /// </summary>
    public virtual ICollection<User> Users { get; set; }
}