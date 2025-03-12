using System;

namespace CodeBase.Data
{
    [Serializable]
    public class Loot
    {
        private int _value;
        
        public int Value => _value;

        public void Init(int value)
        {
            _value = value;
        }
    }
}