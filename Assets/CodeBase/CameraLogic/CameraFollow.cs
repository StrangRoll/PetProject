using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float _rotationAngleX;
    [SerializeField] private float _offset;
    
    private Transform _following;

    public void Follow(Transform following)
    {
        _following = following;
    }
    
    private void LateUpdate()
    {
        if (_following == null)
            return;

        var newRotation = Quaternion.Euler(_rotationAngleX, 0, 0);
        var newPosition = _following.position + transform.forward * -_offset;

        transform.rotation = newRotation;
        transform.position = newPosition;
    }
}
 