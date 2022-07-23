using Auth.Domain.Events;

namespace Auth.Domain.Aggregates;

public abstract class AggregateRoot
{
    #region 跟版本有關

    /// <summary>
    /// Aggregation 是否有更新 ,,
    /// </summary>
    private bool Modification { get; set; }

    /// <summary>
    /// 更新時間
    /// </summary>
    public DateTime UpdateTime { get; private set; }

    /// <summary>
    /// 紀錄 Aggregation 被更新過了
    /// </summary>
    protected void Modify()
    {
        if (Modification == false)
        {
            UpdateTime = DateTime.Now;
            Modification = true;
        }
    }

    #endregion

    #region 跟事件有關

    private readonly List<IDomainEvent> _events = new();
    
    /// <summary>
    /// 事件
    /// </summary>
    public List<IDomainEvent> Events => _events;

    /// <summary>
    /// 增加事件
    /// </summary>
    /// <param name="event"></param>
    protected void AddEvent(IDomainEvent @event)
    {
        Modify();
        _events.Add(@event);
    }

    /// <summary>
    ///  清掉所有 event
    /// </summary>
    public void ClearEvents() => _events.Clear();

    #endregion
    
}