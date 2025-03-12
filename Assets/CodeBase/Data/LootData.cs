using System;

namespace CodeBase.Data
{
    [Serializable] 
    public class LootData
    {
        public int Collected;

        public LootData(int startCollected)
        {
            Collected = startCollected;
        }
        
        public void Collect(Loot loot)
        {
            Collected += loot.Value;
        }
    }
}