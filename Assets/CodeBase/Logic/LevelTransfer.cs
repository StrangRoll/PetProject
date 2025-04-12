using CodeBase.Infrastructure.StateMachine;
using UnityEngine;

namespace CodeBase.Logic
{
    public class LevelTransfer : MonoBehaviour
    {
        [SerializeField] private SceneNamesEnum _tranferToEnum;
        [SerializeField] private BoxCollider _collider;
        
        private string _tranferTo => SceneNames.FromEnum(_tranferToEnum);
        private IGameStateMachine _stateMachine;
        
        public void Init(IGameStateMachine stateMachine) 
            => _stateMachine = stateMachine;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GameTags.Player))
            {
                _collider.enabled = false;
                _stateMachine.Enter<LoadLevelState, string>(_tranferTo);
            }
        }
        
        private void OnDrawGizmos()
        {
            if (_collider == null)
                return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position + _collider.center, _collider.size);
        }
    }
}