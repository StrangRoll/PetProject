using CodeBase.Infrastructure.Services.Input;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Services.Input
{
    public abstract class InputService : IInputService
    {
        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical"; 
        private const string Button = "Attack";

        public abstract Vector2 Axis { get; }

        public bool IsAttackButtonUp() => 
            ReSimpleButtonInput.GetButton(Button);

        protected static Vector2 SimpleAxis() =>
            new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
    }
}