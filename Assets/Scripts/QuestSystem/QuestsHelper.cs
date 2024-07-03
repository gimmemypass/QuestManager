using System.Linq;
using QuestSystem.BluePrints;
using Sirenix.Utilities;
using UnityEditor;
using Random = UnityEngine.Random;

namespace QuestSystem
{
    public static class QuestsHelper
    {
#if UNITY_EDITOR
        public static int GenerateIndexForNextQuest()
        {
            var index = (int)(2 * (Random.value - 0.5f) * 1000_000_000);
            var allQuests = AssetDatabase.FindAssets("t:Quest").Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<Quest>).ToArray();
            while (allQuests.Any(a => a.ID == index))
            {
                index++;
            }
            return index;
        }
#endif

        public static bool IsStepDrawableOnUI(this BaseQuestStep step)
        {
            return !step.Name.IsNullOrWhitespace() && step is not IActionQuestStep;
        }

        public static int GetDrawableQuestStepsCount(this Quest quest)
        {
            var count = 0;
            foreach (var step in quest.QuestSteps)
            {
                if (step.IsStepDrawableOnUI())
                {
                    count++;
                }
            }

            return count;
        }
        
        public static int GetDrawableCompletedQuestStepsCount(this Quest quest)
        {
            var count = 0;
            foreach (var step in quest.QuestSteps)
            {
                if (step.IsCompleted && step.IsStepDrawableOnUI())
                {
                    count++;
                }
            }

            return count;
        }
    }
}