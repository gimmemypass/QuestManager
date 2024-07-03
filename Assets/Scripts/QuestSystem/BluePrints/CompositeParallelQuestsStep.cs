using System;
using Features.QuestSystem;
using UnityEngine;

namespace HECSFramework.Unity.Features.QuestSystem
{
    [Serializable]
    public class CompositeParallelQuestsStep : BaseQuestStep
    {
        [SerializeField]
        private QuestStepsHolder questStepsHolder = new();

        private void OnEnable()
        {
            questStepsHolder.Parent = this;
        }

        public override void Init()
        {
            base.Init();
            foreach (var step in questStepsHolder.QuestSteps)
            {
                step.Setup(); 
                step.Init();
            }
        }

        public override bool IsReady()
        {
            foreach (var step in questStepsHolder.QuestSteps)
            {
                if (!step.IsReady())
                    return false;
            }

            return true;
        }

        public override BaseQuestStep GetCopy()
        {
            var copy = base.GetCopy() as CompositeParallelQuestsStep;
            copy.questStepsHolder = new();
            for (var i = 0; i < questStepsHolder.QuestSteps.Count; i++)
            {
                var step = questStepsHolder.QuestSteps[i];
                copy.questStepsHolder.QuestSteps.Add(step.GetCopy());
            }
            return copy;
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var step in questStepsHolder.QuestSteps)
            {
                step.Dispose(); 
            }
        }
    }
}