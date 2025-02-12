using System;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public Vector3Data Position;
        public PositionOnLevel PositionOnLevel { get; set; }

        public WorldData(PositionOnLevel positionOnLevel)
        {
            PositionOnLevel = positionOnLevel;
        }
    }
}