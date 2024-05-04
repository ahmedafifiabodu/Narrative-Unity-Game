using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _camera;

    [Header("Movement Settings")]
    [SerializeField] private float _movementForce = 1f;

    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _maxSpeed = 5f;

    [Header("Physics Settings")]
    [SerializeField] private float gravity = 9.8f;

    [SerializeField] private float jumpMultiplier = 0.2f;
    [SerializeField] private float fallMultiplier = 1.2f;

    private readonly float _rotationSpeed = 7f;

    internal Vector3 ForceDirection { get; private set; } = Vector3.zero;
    private CharacterController _controller;

    private ServiceLocator _serviceLocator;
    private AudioManager _audioManager;
    private Vector3 lastPosition;

    private float speed;
    private bool isRunning = false;

    private float stepInterval = 0.5f;
    private float nextStepTime;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Instance;
        _serviceLocator.RegisterServiceDontDestoryOnLoad(this);

        speed = _maxSpeed;

        _controller = GetComponent<CharacterController>();
    }

    private void Start() => _audioManager = _serviceLocator.GetService<AudioManager>();

    internal void Move(Vector2 _direction)
    {
        if (_camera == null) return;

        Vector3 _horizontalMovement = _direction.x * _movementForce * GetCameraRight(_camera);
        _horizontalMovement += _direction.y * _movementForce * GetCameraForward(_camera);

        if (isRunning)
            _horizontalMovement *= speed;

        _horizontalMovement = Vector3.ClampMagnitude(_horizontalMovement, speed);

        _controller.Move((_horizontalMovement + new Vector3(0, ForceDirection.y, 0)) * Time.deltaTime);

        if (_horizontalMovement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_horizontalMovement, Vector3.up);
            _controller.transform.rotation = Quaternion.Slerp(_controller.transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);

            if (Time.time >= nextStepTime)
            {
                _audioManager.PlayWalkSFX(_audioManager._walk);
                nextStepTime = Time.time + stepInterval;
            }
        }
        else
        {
            // If the player is not moving, stop the walk SFX and reset the next step time
            _audioManager.StopWalkSFX();
            nextStepTime = Time.time + stepInterval;
        }
    }

    internal void StartRun() => isRunning = true;

    internal void StopRun() => isRunning = false;

    internal void Jump()
    {
        if (_controller.isGrounded)
        {
            ForceDirection = new Vector3(ForceDirection.x, _jumpForce, ForceDirection.z);
            _audioManager.PlayWalkSFX(_audioManager._jump);
        }
    }

    internal void LookAt()
    {
        Vector3 cameraDirection = _camera.transform.forward;
        cameraDirection.y = 0;

        if (cameraDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraDirection, Vector3.up);
            _controller.transform.rotation = Quaternion.Slerp(_controller.transform.rotation, targetRotation, Time.deltaTime);
        }
    }

    #region Helper Methods

    private Vector3 GetCameraRight(Camera camera)
    {
        Vector3 right = camera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private Vector3 GetCameraForward(Camera camera)
    {
        Vector3 forward = camera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    internal void CheckPlayerJumping()
    {
        if (_controller == null) return;

        if (ForceDirection.y > 0)
            ForceDirection = new Vector3(ForceDirection.x, ForceDirection.y - gravity * jumpMultiplier * Time.deltaTime, ForceDirection.z);
        else if (!_controller.isGrounded)
            ForceDirection = new Vector3(ForceDirection.x, ForceDirection.y - gravity * fallMultiplier * Time.deltaTime, ForceDirection.z);
    }

    internal float GetPlayerVelocityMagnitude() => _controller.velocity.magnitude;

    internal bool IsPlayerGrounded() => _controller.isGrounded;

    internal float GetMaxSpeed => _maxSpeed;

    #endregion Helper Methods
}