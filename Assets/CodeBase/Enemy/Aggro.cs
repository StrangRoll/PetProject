using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class Aggro : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private Follow _follow;
        [SerializeField] private float _cooldown;
        
        private Coroutine _aggroCoroutine;
        private bool _hasAggroTarget;

        private void OnEnable()
        {
            _triggerObserver.TriggerEnter += OnTriggerEnter;
            _triggerObserver.TriggerExit += OnTriggerExit;
            
            _follow.enabled = false;
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEnter -= OnTriggerEnter;
            _triggerObserver.TriggerExit -= OnTriggerExit;
        }

        private void OnTriggerExit(Collider obj)
        {
            if (_hasAggroTarget)
            {
                _hasAggroTarget = false;
                _aggroCoroutine = StartCoroutine(SwitchFollowOfAfterCooldown());
            }
        }

        private IEnumerator SwitchFollowOfAfterCooldown()
        {
            yield return new WaitForSeconds(_cooldown);
            SwitchFollowOff();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasAggroTarget)
                return;
            
            _hasAggroTarget = true;
            StopAggroCoroutine();
            SwitchFollowOn();
        }

        private void StopAggroCoroutine()
        {
            if (_aggroCoroutine != null)
            {
                StopCoroutine(_aggroCoroutine);
                _aggroCoroutine = null;
            }
        }

        private void SwitchFollowOn() => 
            _follow.enabled = true;

        private void SwitchFollowOff() => 
            _follow.enabled = false;
    }
}