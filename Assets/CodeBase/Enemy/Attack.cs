using System.Linq;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent((typeof(EnemyAnimator)))]
    public class Attack : MonoBehaviour
    {
        private const float AttackCooldown = 3f;
        
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private float _cleaving = 0.5f;
        [SerializeField] private Transform _attackPosition;
        [SerializeField] private float _damage;
        
        private IGameFactory _factory;
        private Transform _heroTransform;
        private Collider[] _hits = new Collider[1];

        private float _attackCooldown;
        private bool _isAttacking = false;
        private int _layerMask;
        private bool _attackIsActive = false;

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer(LayerName.Player);
            _factory = AllServices.Container.Single<IGameFactory>();
            _factory.HeroCreated += OnHeroCreated;
        }

        private void Update()
        {
            UpdateCooldown();
            
            if (CanAttack())
                OnAttackStart();
        }

        public void DisableAttack() => 
            _attackIsActive = false;

        public void EnableAttack() => 
            _attackIsActive = true;

        private void UpdateCooldown()
        {
            if (CooldownIsUp() == false && _isAttacking == false)
                _attackCooldown -= Time.deltaTime;
        }

        private void OnAttackStart()
        {
            transform.LookAt(_heroTransform);
            _animator.PlayAttack();
            _isAttacking = true;
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug( _attackPosition.position, _cleaving, 2f);
                hit.transform.GetComponent<IHealth>().TakeDamage(_damage);
            }
        }

        private void OnAttackEnded()
        {
            _attackCooldown = AttackCooldown;
            _isAttacking = false;
        }

        private bool Hit(out Collider hit)
        {
            var hitsCount = Physics.OverlapSphereNonAlloc(_attackPosition.position, _cleaving, _hits, _layerMask);

            hit = _hits.FirstOrDefault();
            return hitsCount > 0;
        }


        private void OnHeroCreated()
        {
            _heroTransform = _factory.HeroGameObject.transform;
        }

        private bool CooldownIsUp() => 
            _attackCooldown <= 0;

        private bool CanAttack() => 
            _attackIsActive && _isAttacking == false && CooldownIsUp();
    }
}