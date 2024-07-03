using System;
using Features.QuestSystem.BluePrints;
using UnityEngine;

namespace HECSFramework.Unity.Features.QuestSystem
{
    [Serializable]
    public class DebugQuestStep : BaseQuestStep, IActionQuestStep
    {
        public string DebugMessage;
        public override bool IsReady()
        {
            Debug.Log(DebugMessage);
            return true;
        }
    }
}