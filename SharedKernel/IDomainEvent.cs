namespace SharedKernel;

public interface IDomainEvent
{
    public static DateTime DateOccurred { get; protected set; } 
}
