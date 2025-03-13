using CodeBase;
using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroMove : MonoBehaviour, ISavedProgress
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _movementSpeed;
    
    private IInputService _inputService;
    private Camera _camera;

    private void Awake()
    {
        _inputService = AllServices.Container.Single<IInputService>();
    }

    private void Start()
    {
        _camera = Camera.main;
    }
    
    private void Update()
    {
         var movementVector = Vector3.zero;
         
         if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
         {
             movementVector = _camera.transform.TransformDirection(_inputService.Axis);
             movementVector.y = 0;
             movementVector.Normalize();

             transform.forward = movementVector;
         }

         movementVector += Physics.gravity;
         
         
        _characterController.Move(movementVector * (_movementSpeed * Time.deltaTime));
    }
    
    public void UpdateProgress(PlayerProgress progress)
    {
        progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevelName(), transform.position.AsVector3Data());
    }

    public void LoadProgress(PlayerProgress progress)
    {
        if (progress.WorldData.PositionOnLevel.Level == CurrentLevelName())
        {
            var savedPosition = progress.WorldData.PositionOnLevel.Position;
            
            if (savedPosition != null)
                Warp(savedPosition);
        }
    }

    private void Warp(Vector3Data to)
    {
        _characterController.enabled = false;
        transform.position = to.AsUnityVector3().AddY(_characterController.height);
        _characterController.enabled = true;
    }

    private static string CurrentLevelName() =>
        SceneManager.GetActiveScene().name;
}