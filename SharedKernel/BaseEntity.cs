namespace SharedKernel
{
    public abstract class TEntity<TId> where TId : struct
    {
        public TId Id { get; set; }
        public List<IDomainEvent> Events { get; protected set; } = new();
    }
}
