using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.UI
{
    [RequireComponent(typeof(Button))]
    public class ReSimpleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private string _buttonName;

        private bool _isPressed = false;

        public string Name => _buttonName;
        
        public void OnPointerDown(PointerEventData eventData) => 
            _isPressed = true;

        public void OnPointerUp(PointerEventData eventData) => 
            _isPressed = false;

        public bool GetButton(string button)
        {
            if (button == _buttonName)
                return _isPressed;
            
            return false;
        }

        private void OnButtonClicked()
        {
            _isPressed = !_isPressed;
        }
    }
}