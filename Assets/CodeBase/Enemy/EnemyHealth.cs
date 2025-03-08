using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private float _max;
        
        private float _current;

        public event Action<float, float> HealthChanged;

        public float Max
        {
            get => _max;
            set => _max = value;
        }

        public float Current
        {
            get => _current;
            set => _current = value;
        }

        private void Start()
        {
            _current = _max;
        }

        public void TakeDamage(float damage)
        {
            Current -= damage;
            
            _animator.PlayHit();
            HealthChanged?.Invoke(_current, _max);
        }
    }
}