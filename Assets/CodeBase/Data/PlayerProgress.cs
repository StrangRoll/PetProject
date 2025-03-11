using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public State HeroState;
        public WorldData WorldData;
        public Stats HeroStats;
        public KillData KillData;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(new PositionOnLevel(initialLevel));
            HeroState = new State();
            HeroStats = new Stats();
            KillData = new KillData();
        }
    }
}