using Auth.Domain.Errors;
using MediatR;
using OneOf;

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
    public DateTime? UpdateTime { get; private set; }

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

    private List<IRequest<OneOf<bool,Failure>>> _events = new ();
    
    /// <summary>
    /// 增加事件
    /// </summary>
    /// <param name="event"></param>
    protected void AddEvent(IRequest<OneOf<bool,Failure>> @event)
    {
        Modify();
        _events.Add(@event);
    }

    /// <summary>
    ///  取得所有發生的領域事件
    /// </summary>
    public List<IRequest<OneOf<bool,Failure>>> ClearEvents()
    {
        // Processing - 先把 Events 都複製下來
        var events = new List<IRequest<OneOf<bool,Failure>>>(_events);
        // Processing - 清除 Events
        _events.Clear();
        // Output - 剛剛發生的所有領域事件
        return events;
    }

    #endregion
    
}