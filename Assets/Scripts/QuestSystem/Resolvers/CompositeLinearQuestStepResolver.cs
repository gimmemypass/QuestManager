using System;
using Features.QuestSystem.BluePrints;
using MessagePack;
using Newtonsoft.Json;

namespace HECSFramework.Serialize
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
            // Uncomment when we need save state in composite linear steps
            // data.GoToStep(CurrentStepIndex);
        }
    }
}