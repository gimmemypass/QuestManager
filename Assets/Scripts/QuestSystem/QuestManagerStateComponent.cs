using System;
using QuestSystem.BluePrints;

namespace QuestSystem
{
    public class QuestManagerStateComponent
    {
        private Quest currentStoryQuest;
        
        public event Action<Quest> QuestStarted;
        
        public Quest CurrentStoryQuest => currentStoryQuest;

        public void SetCurrentQuest(Quest quest)
        {
            currentStoryQuest = quest;
        }

        public void QuestStartedCallback(Quest quest)
        {
            QuestStarted?.Invoke(quest);
        }
    }
}