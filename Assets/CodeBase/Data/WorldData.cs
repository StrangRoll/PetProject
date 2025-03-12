using System;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public LootData LootData;
        public PositionOnLevel PositionOnLevel;

        public WorldData(PositionOnLevel positionOnLevel, LootData lootData)
        {
            PositionOnLevel = positionOnLevel;
            LootData = lootData;
        }
    }
}