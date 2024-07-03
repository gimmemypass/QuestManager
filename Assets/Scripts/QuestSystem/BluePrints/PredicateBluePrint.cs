using HECSFramework.Core;
using UnityEngine;

namespace HECSFramework.Unity
{
    
    public abstract class PredicateBluePrint
    public abstract class PredicateBluePrint<TContext> : ScriptableObject, IPredicateContainer<TContext>
    {
        public abstract IPredicate<TContext> GetPredicate(TContext context);
    }
}