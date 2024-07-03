using UnityEngine;

namespace QuestSystem.Predicates
{
    public abstract class PredicateBluePrint<TContext> : ScriptableObject, IPredicateContainer<TContext>
    {
        public abstract IPredicate<TContext> GetPredicate(TContext context);
    }
}