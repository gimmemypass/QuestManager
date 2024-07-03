using System;

namespace HECSFramework.Unity.Features.QuestSystem
{
    [Serializable]
    public struct QuestResult
    {
        public PredicateBluePrint[] Predicates;
        public RewardsBluePrintBase[] Rewards;
    }
}