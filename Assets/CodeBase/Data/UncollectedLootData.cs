using System;

namespace CodeBase.Data
{
    [Serializable]
    public class UncollectedLootData
    {
        public Loot Loot;
        public PositionOnLevel PositionOnLevel;
        
        public UncollectedLootData(Loot loot, PositionOnLevel positionOnLevel)
        {
            Loot = loot;
            PositionOnLevel = positionOnLevel;
        }
    }
}