using System;
using UnityEngine;

namespace QuestSystem.BluePrints
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