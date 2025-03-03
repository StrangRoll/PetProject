using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class RotateToHero : Follow
    {
        [SerializeField] private float _speed;
        
        private Transform _heroTransform;
        private IGameFactory _gameFactory;
        private Vector3 _positionToLook;

        private void Start()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            
            if (HeroExists())
                InitHeroTransform();
            else
                _gameFactory.HeroCreated += OnHeroCreated;
        }
        
        private void Update()
        {
            if (IsHeroInitialized)
                RotateTowardsHero();
        }

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

        private void OnHeroCreated() => 
            InitHeroTransform();

        private void InitHeroTransform() => 
            _heroTransform = _gameFactory.HeroGameObject.transform;

        private bool HeroExists() => 
            _gameFactory.HeroGameObject != null;
        
        private bool IsHeroInitialized => 
            _heroTransform != null;
    }
}