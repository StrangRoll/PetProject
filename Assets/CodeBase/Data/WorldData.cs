using System;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public LootData LootData;
        public PositionOnLevel PositionOnLevel;

        public WorldData(PositionOnLevel positionOnLevel)
        {
            PositionOnLevel = positionOnLevel;
        }
    }
}