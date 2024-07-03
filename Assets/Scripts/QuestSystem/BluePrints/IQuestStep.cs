using System;

namespace QuestSystem.BluePrints
{
    public interface IQuestStep : IDisposable
    {
        public bool IsReady();
    }
}