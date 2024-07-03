using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using QuestSystem.BluePrints;
using QuestSystem.Resolvers;
using UnityEngine;

namespace QuestSystem
{
    [Serializable]
    public sealed class QuestsManager 
    {
        private QuestsManagerComponent questsManagerComponent;
        private QuestManagerStateComponent questManagerStateComponent;
        private PlayerQuestStateComponent playerQuestStateComponent;

        private Quest currentStoryQuest;

        public QuestsManager(
            QuestsManagerComponent questsManagerComponent,
            QuestManagerStateComponent questManagerStateComponent,
            PlayerQuestStateComponent playerQuestStateComponent
            )
        {
            this.questsManagerComponent = questsManagerComponent;
            this.questManagerStateComponent = questManagerStateComponent;
            this.playerQuestStateComponent = playerQuestStateComponent;
        }
        
        public void StartQuest()
        {
            var storyQuest = questsManagerComponent.GetCopyQuest(playerQuestStateComponent.QuestId);
            if (storyQuest == null)
                storyQuest = questsManagerComponent.GetCopyQuest(0);
            else
                ApplyLoadedDataToQuest(storyQuest, playerQuestStateComponent.QuestStepContainers);

            InitLoadedQuest(storyQuest,playerQuestStateComponent.StepIndex,true);
        }

        private void ApplyLoadedDataToQuest(Quest quest, List<string> questStepContainers)
        {
            for (var i = 0; i < quest.QuestSteps.Count && i < questStepContainers.Count; i++)
            {
                var step = quest.QuestSteps[i];
                var container = JsonConvert.DeserializeObject<QuestStepContainer>(questStepContainers[i]);
                new ResolversMap().DeserializeQuestStepContainerToObject(step, container);
            }
        }
        
        private void InitLoadedQuest(Quest quest, int stepIndex, bool needPause)
        {
            Debug.Log($"quest {quest.Name} started");
            questManagerStateComponent.SetCurrentQuest(quest);
            currentStoryQuest = quest;
            currentStoryQuest.Init(stepIndex);
            currentStoryQuest.StepCompletedEvent += StepCompleted;
            currentStoryQuest.QuestCompleted += QuestCompleted;
            currentStoryQuest.Pause(needPause);
        }

        private void StartQuest(Quest quest)
        {
            Debug.Log($"quest {quest.Name} started");
            questManagerStateComponent.SetCurrentQuest(quest);
            currentStoryQuest = quest;
            currentStoryQuest.Init();
            currentStoryQuest.Pause(true);
            currentStoryQuest.StepCompletedEvent += StepCompleted;
            currentStoryQuest.QuestCompleted += QuestCompleted;

            questManagerStateComponent.QuestStartedCallback(quest);
        }

        private void FinishQuest(Quest quest)
        {
            Debug.Log($"quest {quest.Name} completed");
            quest.StepCompletedEvent -= StepCompleted;
            quest.QuestCompleted -= QuestCompleted;           
            quest.Dispose();
        }

        public void UpdateLocal()
        {
            if (currentStoryQuest != null)
                currentStoryQuest.UpdateState();
        }


        private void QuestCompleted(Quest quest)
        {
            FinishQuest(quest);
            if (quest.ID != currentStoryQuest.ID)
                return;
            if (questsManagerComponent.TryGetNextCopyQuest(currentStoryQuest.ID, out var nextQuest))
            {
                StartQuest(nextQuest);
            }
            else
            {
                nextQuest = questsManagerComponent.GetCopyQuest(0);
                StartQuest(nextQuest);
            }
        }

        private void StepCompleted(int stepIndex, Quest quest)
        {
            Debug.Log($"step {currentStoryQuest.QuestSteps[stepIndex].Name} completed");
        }

        public void SkipQuest()
        {
            currentStoryQuest.CompleteQuest();
        }

        public void SkipStepGlobal()
        {
            if (currentStoryQuest.QuestSteps[currentStoryQuest.StepIndex] is CompositeLinearQuestStep linearQuestStep)
            {
                linearQuestStep.CompleteCurrentStep();
                return;
            }
            currentStoryQuest.CompleteCurrentStep();
        }

        public void BeforeSave()
        {
            playerQuestStateComponent.QuestId = questManagerStateComponent.CurrentStoryQuest.ID;
            playerQuestStateComponent.StepIndex = questManagerStateComponent.CurrentStoryQuest.StepIndex;
            playerQuestStateComponent.QuestStepContainers = new List<string>();

            for (var i = 0; i < questManagerStateComponent.CurrentStoryQuest.QuestSteps.Count; i++)
            {
                var step = questManagerStateComponent.CurrentStoryQuest.QuestSteps[i];
                var questStepContainer = new ResolversMap().GetQuestStepResolverContainer(step);
                playerQuestStateComponent.QuestStepContainers.Add(questStepContainer);
            }
        }
    }
}