namespace CodeBase.Data
{
    public class PositionOnLevel
    {
        public string Level;
        public Vector3Data Position;
        
        public PositionOnLevel(string level, Vector3Data position = null)
        {
            Level = level;
            Position = position;
        }
    }
}