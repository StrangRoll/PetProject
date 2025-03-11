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
        
        private void Update()
        {
            if (HeroNotReached())
                _agent.destination = _heroTransform.position;
        }

        public void Init(Transform heroTransform) => 
            _heroTransform = heroTransform;

        private bool HeroNotReached() => 
            Vector3.Distance(_agent.transform.position, _heroTransform.position) > MinDistance;
    }
}