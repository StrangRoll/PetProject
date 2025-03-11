using UnityEngine;

namespace CodeBase.Enemy
{
    public class RotateToHero : Follow
    {
        [SerializeField] private float _speed;
        
        private Transform _heroTransform;
        private Vector3 _positionToLook;
        
        private void Update()
        {
            if (IsHeroInitialized)
                RotateTowardsHero();
        }

        public void Init(Transform heroTransform) => 
            _heroTransform = heroTransform;

        private void RotateTowardsHero()
        {
            UpdatePositionToLookAt();
            transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
        }

        private Quaternion SmoothedRotation(Quaternion transformRotation, Vector3 vector3) => 
            Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_positionToLook), _speed * Time.deltaTime); 

        private void UpdatePositionToLookAt()
        {
            var positionDiff = _heroTransform.position - transform.position;
            _positionToLook = new Vector3(positionDiff.x, 0, positionDiff.z);
        }
        
        private bool IsHeroInitialized => 
            _heroTransform != null;
    }
}