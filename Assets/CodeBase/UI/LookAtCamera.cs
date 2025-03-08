using UnityEngine;

namespace CodeBase.UI
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            var cameraRotation = _mainCamera.transform.rotation;
            transform.LookAt(transform.position + cameraRotation * Vector3.back, cameraRotation * Vector3.up);
        }
    }
}