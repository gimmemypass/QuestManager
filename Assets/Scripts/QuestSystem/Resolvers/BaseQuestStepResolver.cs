using System;
using HECSFramework.Unity.Features.QuestSystem;
using MessagePack;
using Newtonsoft.Json;

namespace HECSFramework.Serialize
{
    [Serializable, JsonObject]
    public struct BaseQuestStepResolver: IQuestStepResolver<BaseQuestStep, BaseQuestStepResolver>
    {
        [Key(0)]
        public bool IsCompleted;
        
        public BaseQuestStepResolver In(ref BaseQuestStep data)
        {
            IsCompleted = data.IsCompleted;
            return this;
        }

        public void Out(ref BaseQuestStep data)
        {
            data.SetCompleted(IsCompleted);
        }
    }
    
    public interface IQuestStepResolver { }
    
    public interface IQuestStepResolver<TypeToResolve, Resolver> : IQuestStepResolver
    {
        Resolver In(ref TypeToResolve data);
        void Out(ref TypeToResolve data);
    }
}