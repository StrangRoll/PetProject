using System;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;

        public WorldData(PositionOnLevel positionOnLevel)
        {
            PositionOnLevel = positionOnLevel;
        }
    }
}