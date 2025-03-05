using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Attack))]
    public class CheckAttackRange : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        
        private Attack _attack;

        private void Awake()
        {
            _attack = GetComponent<Attack>();
        }

        private void OnEnable()
        { 
            _triggerObserver.TriggerEnter += OnTriggerEnter;
            _triggerObserver.TriggerExit += OnTriggerExit;
        }

        private void OnTriggerExit(Collider obj)
        {
            _attack.DisableAttack();
        }

        private void OnTriggerEnter(Collider obj)
        {
            _attack.EnableAttack();
        }
    }
}