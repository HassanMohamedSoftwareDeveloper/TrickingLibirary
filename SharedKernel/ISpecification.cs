using System.Linq.Expressions;

namespace SharedKernel;

public interface ISpecification<TEntity> where TEntity : class
{
    //Expression<Func<TEntity, bool>>? FilterExpression { get; }
    //List<Expression<Func<TEntity, object>>> IncludeExpressions { get; }
    //List<OrderByParam<TEntity>> OrderByExpressions { get; }
}
