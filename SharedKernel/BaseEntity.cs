namespace SharedKernel
{
    public abstract class BaseEntity<TId> 
    {
        public TId Id { get; set; }
        //public List<IDomainEvent> Events { get; protected set; } = new();
    }
}
