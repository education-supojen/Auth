namespace Auth.Domain.Aggregates;

public class Schedule : IEntityBase<long>
{
    public Schedule() { }
    
    public Schedule(string name, DayOfWeek[] weekend)
    {
        Name = name;
        Weekend = weekend;
    }
    
    /// <summary>
    /// 工作表 ID
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// 工作表名稱
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 週休安排
    /// </summary>
    public DayOfWeek[] Weekend { get; init; }
    
    /// <summary>
    /// 屬於哪家公司
    /// </summary>
    public virtual Company Company { get; set; }
    
    /// <summary>
    /// 使用者
    /// </summary>
    public virtual ICollection<User> Users { get; set; }
}