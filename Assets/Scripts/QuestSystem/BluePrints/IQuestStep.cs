using System;
using HECSFramework.Core;

namespace HECSFramework.Unity.Features.QuestSystem
{
    public interface IQuestStep : IDisposable
    {
        public bool IsReady();
    }
}