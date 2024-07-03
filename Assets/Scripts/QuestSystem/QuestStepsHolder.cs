using System;
using System.Collections.Generic;
using Features.QuestSystem.BluePrints;
using HECSFramework.Unity.Features.QuestSystem;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Features.QuestSystem
{
    [Serializable]
    public class QuestStepsHolder
    {
        [HideInInspector]
        public Object Parent;
        
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        [ListDrawerSettings(HideAddButton = true, CustomRemoveElementFunction = nameof(RemoveStep), NumberOfItemsPerPage = 99, ElementColor = nameof(GetQuestStepColor))]
        [SerializeField]
        public List<BaseQuestStep> QuestSteps = new();

        private Color GetQuestStepColor(int index, Color defaultColor, List<BaseQuestStep> list)
        {
            if (list[index] is IActionQuestStep)
            {
                return new Color32(44, 105, 117, 255);
            }

            return defaultColor;
        }
        
        private void RemoveStep(BaseQuestStep step)
        {
            QuestSteps.Remove(step);
#if UNITY_EDITOR
            ClearDeletedBluePrint(step);
#endif
        }
        
#if UNITY_EDITOR

        [Button]
        [InfoBox("You need create SO first", InfoMessageType.Warning, "@!IsValid()")]
        [EnableIf(nameof(IsValid))]
        private void AddStep()
        {
            var window = EditorWindow.GetWindow<AddQuestStepWindow>();
            window.Init(Parent, AddStep);
        }
        private bool IsValid()
        {
            return EditorUtility.IsPersistent(Parent);
        }
        private void AddStep(BaseQuestStep questStep)
        {
            QuestSteps.Add(questStep);
        }
        private void ClearDeletedBluePrint(BaseQuestStep stepToRemove)
        {
            QuestSteps.RemoveAll(a => a == null);
            AssetDatabase.RemoveObjectFromAsset(stepToRemove);
            Object.DestroyImmediate(stepToRemove);
            AssetDatabase.SaveAssets();
        }
#endif

    }
}