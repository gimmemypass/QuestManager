using System.Linq;
using HECSFramework.Unity.Features.QuestSystem;
using UnityEngine;

namespace Features.QuestSystem.BluePrints
{
    public class CompositeLinearQuestStep : BaseQuestStep
    {
        [SerializeField]
        private QuestStepsHolder questStepsHolder = new();

        private bool needChangeStep;
        private int currentStepIndex = 0;

        public int CurrentStepIndex => currentStepIndex;

        private void OnEnable()
        {
            questStepsHolder.Parent = this;
        }

        public void GoToStep(int stepIndex)
        {
            needChangeStep = true;
            currentStepIndex = stepIndex - 1;
        }

        public override void Init()
        {
            base.Init();
            var firstStep = questStepsHolder.QuestSteps.First();
            firstStep.Setup();
            firstStep.Init();
        }

        public override bool IsReady()
        {
            if (needChangeStep)
            {
                currentStepIndex++;
                
                if (currentStepIndex >= questStepsHolder.QuestSteps.Count)
                    return true;
                
                var nextStep = questStepsHolder.QuestSteps[currentStepIndex];
                nextStep.Setup();
                nextStep.Init();
                needChangeStep = false;
                return false;
            }
            
            if (questStepsHolder.QuestSteps[currentStepIndex].IsReady())
            {
                needChangeStep = true;
            }

            return false;
        }

        public void CompleteCurrentStep()
        {
            needChangeStep = true;
        }
        
        public override BaseQuestStep GetCopy()
        {
            var copy = base.GetCopy() as CompositeLinearQuestStep;
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
            currentStepIndex = 0;
            foreach (var step in questStepsHolder.QuestSteps)
            {
                step.Dispose(); 
            }
        }
    }
}