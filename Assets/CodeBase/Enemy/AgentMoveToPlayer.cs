using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class AgentMoveToPlayer : Follow
    {
        private const float MinDistance = 1;
        
        [SerializeField] 
        private NavMeshAgent _agent;
        
        private Transform _heroTransform;
        private IGameFactory _gameFactory;
        private bool IsHeroInitialized = false;
            
        private void Start()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            
            if (_gameFactory.HeroGameObject != null)
                InitHeroTransform();
            else
                _gameFactory.HeroCreated += OnHeroCreated;
        }

        private void Update()
        {
            if (IsHeroInitialized == false) 
                return;
            
            if (HeroNotReached())
                _agent.destination = _heroTransform.position;
        }

        private void OnHeroCreated()
        {
            InitHeroTransform();
            _gameFactory.HeroCreated -= OnHeroCreated;
        }

        private void InitHeroTransform()
        {
            _heroTransform = _gameFactory.HeroGameObject.transform;
            IsHeroInitialized = true;
        }


        private bool HeroNotReached() => 
            Vector3.Distance(_agent.transform.position, _heroTransform.position) > MinDistance;
    }
}