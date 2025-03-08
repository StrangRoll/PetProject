using CodeBase.Hero;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] private HpBar _hpBar;

        private IHealth _heroHealth;
        
        private void OnDestroy()
        {
            _heroHealth.HealthChanged -= OnHealthChanged;
        }

        public void Init(HeroHealth heroHealth)
        {
            _heroHealth = heroHealth;
            
            _heroHealth.HealthChanged += OnHealthChanged;
        }

        private void UpdateHpBar(float current, float max)
        {
            _hpBar.SetValue(current, max);
        }

        private void OnHealthChanged(float current, float max) => 
            UpdateHpBar(current, max);
    }
}