using UnityEngine;

namespace QuestSystem.BluePrints
{
    public abstract partial class BaseQuestStep : ScriptableObject, IQuestStep
    {
        public string Name;
        public bool IsCompleted { get; private set; }

        public virtual void Setup(){}

        public virtual void Init(){}

        public abstract bool IsReady();

        public virtual BaseQuestStep GetCopy()
        {
            return ScriptableObject.Instantiate(this);
        }
        public virtual void Dispose()
        {
            IsCompleted = true;
        }

        public void SetCompleted(bool isCompleted)
        {
            IsCompleted = isCompleted;
        }
    }
}