using System;
using System.Collections.Generic;
using QuestSystem.Utils;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuestSystem.BluePrints
{
    public class Quest : ScriptableObject
    {
        public event Action<int, Quest> StepCompletedEvent;
        public event Action<Quest> QuestCompleted;

        public int ID => id;
        public string Name => questName;
        public string Description => description;
        public int StepIndex => questStepsHolder.QuestSteps.IndexOf(currentStep);
        
        [ReadOnly]
        [SerializeField]
        [BoxGroup("Id")]
        private int id;

        [SerializeField]
#if UNITY_EDITOR
        [InlineButton(nameof(UpdateAssetName))]
#endif
        private string questName;

        [SerializeField] private string description;
        [SerializeField] private Sprite icon;
        
        [SerializeField]
        private QuestStepsHolder questStepsHolder = new();
        
        private BaseQuestStep currentStep;
        private bool isPaused = false;
        private bool needChangeStep;
        private bool needInitStep;

        public IReadOnlyList<BaseQuestStep> QuestSteps => questStepsHolder.QuestSteps;
        public bool IsPaused => isPaused;

        public void Construct()
        {
            GenerateIndex(); 
        }
        public void Init(int stepIndex = 0)
        {
            needInitStep = true;
            
            currentStep = questStepsHolder.QuestSteps[stepIndex];
        }

        public void RestartCurrentStep()
        {
            var stepIndex = questStepsHolder.QuestSteps.IndexOf(currentStep);
            GoToStep(stepIndex);
        }
        
        public void GoToStep(int stepIndex)
        {
            StartStep(stepIndex);
        }

        public void CompleteCurrentStep()
        {
            if (needChangeStep)
                return;
            needChangeStep = true;
            var stepIndex = questStepsHolder.QuestSteps.IndexOf(currentStep);
            currentStep.Dispose();
            StepCompletedEvent?.Invoke(stepIndex, this);
        }

        public void CompleteQuest()
        {
            QuestCompleted?.Invoke(this);
        }

        public Quest GetDeepCopy()
        {
            var copy = Object.Instantiate(this);
            for (int i = 0; i < copy.questStepsHolder.QuestSteps.Count; i++)
            {
                copy.questStepsHolder.QuestSteps[i] = copy.questStepsHolder.QuestSteps[i].GetCopy();
            }
            
            return copy;
        }

        private void OnEnable()
        {
            questStepsHolder.Parent = this;
        }

        public void UpdateState()
        {
            if (isPaused)
                return;
            if (needInitStep)
            {
                StartStep(StepIndex);
                
                needInitStep = false;
            }
            
            if (needChangeStep)
            {
                var stepIndex = currentStep == null ? -1 : questStepsHolder.QuestSteps.IndexOf(currentStep);
                currentStep = null;
                
                if (questStepsHolder.QuestSteps.Count > stepIndex + 1)
                {
                    StartStep(stepIndex + 1);
                }
                else
                {
                    QuestCompleted?.Invoke(this);
                }

                needChangeStep = false;
            }
            if (questStepsHolder.QuestSteps.Count == 0)
            {
                QuestCompleted?.Invoke(this);
            }
            if (currentStep == null)
            {
                return;
            }

            if (currentStep.IsReady())
            {
                CompleteCurrentStep();
            }
        }

        private void StartStep(int stepIndex)
        {
            if (currentStep != null)
                currentStep.Dispose();
            currentStep = questStepsHolder.QuestSteps[stepIndex];
            currentStep.Setup();
            currentStep.Init();
        }

        public void Pause(bool pause)
        {
            if (isPaused == pause)
                return;

            if (pause)
                Pause();
            else
                UnPause();
        }

        private void Pause()
        {
            isPaused = true;
        }

        private void UnPause()
        {
            isPaused = false;
        }

        public void Dispose()
        {
            if (currentStep != null)
                currentStep.Dispose();
        }
#if UNITY_EDITOR
        private void UpdateAssetName()
        {
            var path = AssetDatabase.GetAssetPath(this);
            AssetDatabase.RenameAsset(path, $"{Name}_{ID}");
        }
#endif

        [BoxGroup("Id")]
        [Button]
        private void GenerateIndex()
        {
#if UNITY_EDITOR
            id = QuestsHelper.GenerateIndexForNextQuest();
#endif
        }
    }
}