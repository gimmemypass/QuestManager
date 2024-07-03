using System;
using MessagePack;
using Newtonsoft.Json;
using QuestSystem.BluePrints;

namespace QuestSystem.Resolvers
{
    [Serializable, JsonObject]
    public struct CompositeLinearQuestStepResolver: IQuestStepResolver<CompositeLinearQuestStep, CompositeLinearQuestStepResolver>
    {
        [Key(0)]
        public bool IsCompleted;
        [Key(1)]
        public int CurrentStepIndex;
        
        public CompositeLinearQuestStepResolver In(ref CompositeLinearQuestStep data)
        {
            IsCompleted = data.IsCompleted;
            CurrentStepIndex = data.CurrentStepIndex;
            return this;
        }

        public void Out(ref CompositeLinearQuestStep data)
        {
            data.SetCompleted(IsCompleted);
        }
    }
}