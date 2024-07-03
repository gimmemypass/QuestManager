using System;
using System.Collections.Generic;
using QuestSystem.BluePrints;
using UnityEngine;

namespace QuestSystem.Data
{
    [Serializable]
    public sealed class QuestsManagerComponent
    {
        [SerializeField] private Quest[] storyQuests;

        public IReadOnlyList<Quest> StoryQuests => storyQuests;

        public Quest GetQuest(int id)
        {
            var quest = FindQuest(id, out _); 
            return quest;
        }
        
        public Quest GetCopyQuest(int id)
        {
            var quest = FindQuest(id, out _);
            return quest.GetDeepCopy();
        }

        public bool TryGetNextCopyQuest(int id, out Quest quest)
        {
            FindQuest(id, out var index);
            if (storyQuests.Length > index + 1)
            {
                quest = storyQuests[index + 1].GetDeepCopy();
                return true;
            }

            quest = default;
            return false;
        }

        public List<Quest> GetNextNQuests(int id, int count)
        {
            var questsList = new List<Quest>(); 
            FindQuest(id, out var index);
            while (storyQuests.Length > index + 1 && count > questsList.Count)
            {
                questsList.Add(storyQuests[index + 1]);
                index++;
            }

            return questsList;
        }

        private Quest FindQuest(int id, out int index)
        {
            for (int i = 0; i < storyQuests.Length; i++)
            {
                if (storyQuests[i].ID == id)
                {
                    index = i;
                    return storyQuests[i];
                }
            }

            index = -1;
            return storyQuests[0];
        }
    }
}