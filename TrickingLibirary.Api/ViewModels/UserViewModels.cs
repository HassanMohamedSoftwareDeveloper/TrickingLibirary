using System.Linq.Expressions;
using TrickingLibirary.Domain.Entities;

namespace TrickingLibirary.Api.ViewModels;

public static class UserViewModels
{
    //public static readonly Func<User, object> CreateFlat = FlatProjection.Compile();
    public static object CreateFlat(User user) => FlatProjection.Compile()(user);
    public static Expression<Func<User, object>> FlatProjection =>
        user => new
        {
            user.Username,
            user.Image
        };
}
