using CodeBase.Hero;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] private HpBar _hpBar;

        private IHealth _health;
        
        private void OnDestroy()
        {
            _health.HealthChanged -= OnHealthChanged;
        }

        public void Init(IHealth heroHealth)
        {
            _health = heroHealth;
            
            _health.HealthChanged += OnHealthChanged;
        }

        private void Start()
        {
            TestCheckIHealth();
        }

        private void TestCheckIHealth()
        {
            if (_health == null)
            {
                _health = GetComponentInChildren<IHealth>();
                _health.HealthChanged += OnHealthChanged;
            }
        }

        private void UpdateHpBar(float current, float max)
        {
            _hpBar.SetValue(current, max);
        }

        private void OnHealthChanged(float current, float max) => 
            UpdateHpBar(current, max);
    }
}