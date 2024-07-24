using System.Linq.Expressions;

namespace SchoolSystemTask.BAL.Specefication
{
    public interface ISpecefication<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }


    }
}
