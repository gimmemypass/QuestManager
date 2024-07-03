using System;
using System.Collections.Generic;
using MessagePack;

namespace QuestSystem.Data
{
    [Serializable]
    [MessagePackObject]
    public sealed class PlayerQuestStateComponent
    {
        [Key(0)]
        public int QuestId;
        [Key(1)]
        public int StepIndex;
        [Key(2)] 
        public List<string> QuestStepContainers = new();
    }
}