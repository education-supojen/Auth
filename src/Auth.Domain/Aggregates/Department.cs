namespace Auth.Domain.Aggregates;

public class Department : IEntityBase<long>
{

    private Department() { }

    public Department(string name) => Name = name;

    /// <summary>
    /// 部門 ID
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// 部門名稱
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 屬於哪家公司
    /// </summary>
    public virtual Company Company { get; set; }

    /// <summary>
    /// 使用者
    /// </summary>
    public virtual ICollection<User> Users { get; set; }
}