using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private WindowId _windowId;
        
        private IWindowService _windowService;

        public void Init(IWindowService windowService) => 
            _windowService = windowService;

        private void OnEnable() => 
            _button.onClick.AddListener(Open);

        private void OnDisable() => 
            _button.onClick.RemoveListener(Open);

        private void Open() => 
            _windowService.Open(_windowId);
    }
}