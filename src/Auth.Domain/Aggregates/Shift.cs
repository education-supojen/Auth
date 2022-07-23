namespace Auth.Domain.Aggregates;

public class Shift : IEntityBase<long>
{
    public Shift() { }

    public Shift(string name, TimeOnly start, TimeOnly end, TimeOnly breakStart, TimeOnly breakEnd, int flexibleRange)
    {
        Name = name;
        StartTime = start;
        EndTime = end;
        StartBreakTime = breakStart;
        EndBreakTime = breakEnd;
        FlexibleRange = flexibleRange;
    }

    /// <summary>
    /// 輪班 ID
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// 輪班名稱
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 上班時間
    /// </summary>
    public TimeOnly StartTime { get; set; }

    /// <summary>
    /// 下班時間
    /// </summary>
    public TimeOnly EndTime { get; set; }

    /// <summary>
    /// 開始休息時間
    /// </summary>
    public TimeOnly StartBreakTime { get; set; }

    /// <summary>
    /// 結束休息時間
    /// </summary>
    public TimeOnly EndBreakTime { get; set; }

    /// <summary>
    /// 彈性範圍(分鐘)
    /// </summary>
    public int FlexibleRange { get; set; }
    
    /// <summary>
    /// 屬於哪家公司
    /// </summary>
    public virtual Company Company { get; set; }
    
    /// <summary>
    /// 使用者
    /// </summary>
    public virtual ICollection<User> Users { get; set; }
}