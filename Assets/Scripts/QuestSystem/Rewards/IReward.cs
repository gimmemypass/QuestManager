namespace HECSFramework.Rewards
{
    public partial interface IReward
    {
        void Award(IRewardable entity);
    }
}