using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _hitPoint;
        [SerializeField] private int _maxEnemiesCount;
        
        private IInputService _input;
        private int _layerMask;
        private Collider[] _hits;
        private Stats _stats;

        private void Awake()
        {
            _input = AllServices.Container.Single<IInputService>();

            _layerMask = 1 << LayerMask.NameToLayer(LayerName.Hittable);
        }
        
        private void Start()
        {
            _hits = new Collider[_maxEnemiesCount];
        }

        private void Update()
        {
            if (_input.IsAttackButtonUp() && _animator.IsAttacking)
                _animator.PlayAttack();
        }

        public void LoadProgress(PlayerProgress progress) => 
            _stats = progress.HeroStats;

        public void OnAttack()
        {
            for (var i = 0; i < Hit(); i++) 
                _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
        }

        private int Hit() => 
            Physics.OverlapSphereNonAlloc(_hitPoint.position, _stats.DamageRadius, _hits, _layerMask);
    }
}