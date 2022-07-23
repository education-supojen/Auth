namespace Auth.Domain.Aggregates;

public interface IEntityBase<T>
{
    /// <summary>
    /// 每一個 entity 都必須要有一個獨一無二的 Identity
    /// </summary>
    public T Id { get; init; }
}