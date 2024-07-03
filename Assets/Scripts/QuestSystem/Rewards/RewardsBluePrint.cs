using UnityEngine;

namespace QuestSystem.Rewards
{
    public abstract class RewardsBluePrintBase : ScriptableObject
    {
        public abstract IReward GetReward();
    }

    public abstract class RewardsBluePrint<TRewardType> : RewardsBluePrintBase where TRewardType : IReward
    {
        [SerializeField] private TRewardType reward;

        public override IReward GetReward() => reward;
    }
}