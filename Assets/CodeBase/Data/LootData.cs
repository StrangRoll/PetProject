using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable] 
    public class LootData
    {
        public List<UncollectedLootData> UncollectedLoot; 
        public int Collected;
        
        public Action Changed;
        
        public LootData(int startCollected)
        {
            Collected = startCollected;
            UncollectedLoot = new List<UncollectedLootData>();
        }
        
        public void Collect(Loot loot)
        {
            Collected += loot.Value;
            Changed?.Invoke();
        }
        
        public void AddUncollectedLoot(Loot loot, PositionOnLevel positionOnLevel)
        {
            var uncollectedLoot = new UncollectedLootData(loot, positionOnLevel);
            UncollectedLoot.Add(uncollectedLoot);
        }

        public void ClearUncollectedLoot() => UncollectedLoot.Clear();
    }
}