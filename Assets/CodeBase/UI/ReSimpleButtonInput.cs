using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.UI
{
    public static class ReSimpleButtonInput
    {
        private static Dictionary<string, ReSimpleButton> _buttons = new Dictionary<string, ReSimpleButton>();
        
        public static void UpdateButtonsUpdateButtons()
        {
            var simpleButtons = GameObject.FindObjectsOfType<ReSimpleButton>();

            foreach (var button in simpleButtons) 
                _buttons.Add(button.Name, button);
        }
        
        public static bool GetButton(string button) => 
            _buttons[button].GetButton(button);
    }
}