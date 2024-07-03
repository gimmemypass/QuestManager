namespace HECSFramework.Core
{
    public partial interface IPredicate<TContext>
    {
        bool IsReady(TContext context);
    }

    public interface IPredicateContainer<TContext>
    {
        IPredicate<TContext> GetPredicate(TContext context);
    }
}