using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public abstract class WindowBase : MonoBehaviour
    {
        protected IPersistentProgressService ProgressService;
        protected PlayerProgress PlayerProgress => ProgressService.Progress;
        
        [SerializeField] private Button _closeButton;

        public void Construct(IPersistentProgressService progressService) => 
            ProgressService = progressService;
        
        private void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            Init();
            SubscribeUpdates();
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        protected virtual void OnAwake() => 
            _closeButton.onClick.AddListener(() => CloseWindow());

        protected virtual void Init()
        {
            
        }

        protected virtual void SubscribeUpdates()
        {
            
        }

        protected virtual void Cleanup()
        {
            
        } 

        private void CloseWindow() => 
            Destroy(gameObject);
    }
}