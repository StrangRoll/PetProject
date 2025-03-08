using System;

namespace CodeBase.Logic
{
    public interface IHealth
    {
        event Action<float, float> HealthChanged;
        float Max { get; set; }
        float Current { get; set; }
        void TakeDamage(float damage);
    }
}