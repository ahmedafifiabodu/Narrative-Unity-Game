using Cinemachine;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _freeLookCamera;
    private InputSystem _playerInput;
    internal InputSystem.PlayerActions PlayerActions { get; private set; }

    private ServiceLocator _serviceLocator;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Instance;
        _serviceLocator.RegisterServiceDontDestoryOnLoad(this);

        _playerInput = new InputSystem();
        PlayerActions = _playerInput.Player;
    }

    private void OnEnable()
    {
        _playerInput.Enable();

        if (_freeLookCamera != null)
            _freeLookCamera.enabled = true;
    }

    private void OnDisable()
    {
        _playerInput.Disable();

        if (_freeLookCamera != null)
            _freeLookCamera.enabled = false;

        _serviceLocator.GetService<AudioManager>().StopAllAudio();
    }

    private void Start()
    {
        _playerMovement = _serviceLocator.GetService<PlayerMovement>();

        PlayerActions.Run.started += _ => _playerMovement.StartRun();
        PlayerActions.Run.canceled += _ => _playerMovement.StopRun();

        PlayerActions.Jump.performed += _ => _playerMovement.Jump();
        PlayerActions.Quest.performed += _ => _serviceLocator.GetService<UISystem>().ToggleQuestPanel();
    }

    private void FixedUpdate()
    {
        _playerMovement.Move(PlayerActions.Movement.ReadValue<Vector2>());
        //_playerMovement.LookAt();
        _playerMovement.CheckPlayerJumping();
    }
}