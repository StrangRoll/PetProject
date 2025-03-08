using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator))]
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _health;
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private ParticleSystem _deathFX;
        [SerializeField] private float _timeToDestroyCorpse;

        public event Action EnemyDied;
        
        private void OnEnable() => 
            _health.HealthChanged += OnHealthChanged;

        private void OnDisable() => 
            _health.HealthChanged -= OnHealthChanged;

        private void OnHealthChanged(float current, float max)
        {
            if (current <= 0)
                Die();
        }

        private void Die()
        {
            _health.HealthChanged -= OnHealthChanged;
            _animator.PlayDeath();
            Instantiate(_deathFX, transform.position, Quaternion.identity);
            EnemyDied?.Invoke();

            StartCoroutine(CorpseDestroyTimer());
        }

        private IEnumerator CorpseDestroyTimer()
        {
            yield return new WaitForSeconds(_timeToDestroyCorpse);
            Destroy(gameObject);
        }
    }
}