using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroHealth))]
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField] private HeroHealth _health;
        [SerializeField] private HeroMove _move;
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private ParticleSystem _deathFX;
        [SerializeField] private HeroAttack _attack;
        
        private bool _isDead = false;
        
        private void OnEnable() => 
            _health.HealthChanged += OnHealthChanged;

        private void OnDisable() => 
            _health.HealthChanged -= OnHealthChanged;

        private void OnHealthChanged(float arg1, float arg2)
        {
            if (_isDead == false && _health.Currrent <= 0)
                Die();
        }

        private void Die()
        {
            _isDead = true;
            _move.enabled = false;
            _attack.enabled = false;
            _animator.PlayDeath();

            Instantiate(_deathFX, transform.position, Quaternion.identity);
        }
    }
}