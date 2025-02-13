using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Logic
{
    public class SaveTrigger : MonoBehaviour
    {
        [SerializeField] private BoxCollider _collider;
        
        private ISaveLoadService _saveLoadService;

        private void Awake()
        {
             _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        private void OnTriggerEnter(Collider other)
        {
            _saveLoadService.SaveProgress();
            
            Debug.Log("Progress saved");
            gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if (_collider == null)
                return;
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(transform.position + _collider.center, _collider.size);
        }
    } 
}