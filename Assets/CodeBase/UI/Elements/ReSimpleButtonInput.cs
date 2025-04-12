using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public static class ReSimpleButtonInput
    {
        private static Dictionary<string, ReSimpleButton> _buttons = new Dictionary<string, ReSimpleButton>();
        
        public static void UpdateButtons()
        {
            ResetButtons();
            var simpleButtons = GameObject.FindObjectsOfType<ReSimpleButton>();

            foreach (var button in simpleButtons)
            {
                if (_buttons.ContainsKey(button.Name) || button == null) continue;
                _buttons.Add(button.Name, button);
            }
        }
        
        public static bool GetButton(string button) => 
            _buttons[button].GetButton(button);

        private static void ResetButtons() =>
            _buttons = new Dictionary<string, ReSimpleButton>();
    }
}