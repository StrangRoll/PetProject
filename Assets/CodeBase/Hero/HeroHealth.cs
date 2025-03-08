using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
    {
        [SerializeField] private HeroAnimator _animator;
        
        private State _state;
        
        public event Action<float, float> HealthChanged;

        public float Currrent
        {
            get => _state.CurrentHP;
            set
            {
                if (_state.CurrentHP == value)
                    return;
                
                _state.CurrentHP = value;
                HealthChanged?.Invoke(value, _state.MaxHP);
            }
        }
        
        public float Max
        {
            get => _state.MaxHP;
            set => _state.MaxHP = value;
        }

        public float Current { get; set; }

        public void LoadProgress(PlayerProgress progress)
        {
            _state = progress.HeroState;
            HealthChanged?.Invoke(_state.CurrentHP, _state.MaxHP);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroState.CurrentHP = Currrent;
            progress.HeroState.MaxHP = Max;
        }

        public void TakeDamage(float damage)
        {
            if (Currrent <= 0)
                return;

            Currrent -= damage;
            
            _animator.PlayHit();
        }
    }
}