using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCNavMesh : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _delayBetweenPoints = 0.5f;

    [SerializeField] private bool _randomPoints = false;
    [SerializeField] private Transform[] _points;
    [SerializeField] private Transform _targetPoint;

    private float timer;
    private int destPoint = 0;
    private float previousSpeed = 0f;
    private bool goToTarget = false;

    private void Awake()
    {
        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();

        if (_animator == null)
            _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _agent.autoBraking = false;
        timer = _delayBetweenPoints;
        GotoNextPoint();
    }

    private void GotoNextPoint()
    {
        if (_points.Length == 0 || goToTarget)
            return;

        if (timer < _delayBetweenPoints)
            timer += Time.deltaTime;
        else
        {
            if (_randomPoints)
                destPoint = Random.Range(0, _points.Length);
            else
                destPoint = (destPoint + 1) % _points.Length;

            _agent.destination = _points[destPoint].position;
            timer = 0;
        }
    }

    private void Update()
    {
        float currentSpeed = _agent.velocity.magnitude / _agent.speed;
        float smoothSpeed = Mathf.Lerp(previousSpeed, currentSpeed, Time.deltaTime * 2);
        smoothSpeed = Mathf.Min(smoothSpeed, 0.5f);

        _animator.SetFloat(GameConstant.Speed, smoothSpeed);

        _agent.isStopped = false;

        if (!_agent.pathPending && _agent.remainingDistance < 0.1f)
            GotoNextPoint();

        if (goToTarget && !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            Transform _playerTransform = ServiceLocator.Instance.GetService<PlayerMovement>().gameObject.transform;
            FacePlayer(_playerTransform);
        }

        previousSpeed = smoothSpeed;
    }

    #region Helper Functions

    private void FacePlayer(Transform _playerTransform)
    {
        Vector3 directionToPlayer = (_playerTransform.position - transform.position);
        directionToPlayer.y = 0;
        directionToPlayer = directionToPlayer.normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

        float rotationSpeed = 2f;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    public void GoToTargetPoint()
    {
        if (_targetPoint == null)
            return;

        goToTarget = true;

        _agent.destination = _targetPoint.position;
        _agent.stoppingDistance = 0.1f;
    }

    public void ActivateGotoNextPoint()
    {
        goToTarget = false;
        _agent.stoppingDistance = 0f;
        _agent.isStopped = false;

        GotoNextPoint();
    }

    public void LookAtPlayerAndMoveTowardsPlayer()
    {
        Transform _playerTransform = ServiceLocator.Instance.GetService<PlayerLocation>().gameObject.transform;

        Vector3 destination = _playerTransform.position + _playerTransform.forward * 2;

        if (NavMesh.SamplePosition(destination, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            _agent.destination = hit.position;
        else
        {
            if (NavMesh.SamplePosition(destination, out hit, 5.0f, NavMesh.AllAreas))
                _agent.destination = hit.position;
            else
            {
                Debug.Log("No valid position found within radius");
                return;
            }
        }

        goToTarget = true;
        _agent.isStopped = true;
        FacePlayer(_playerTransform);
    }

    public void LookAtPlayer()
    {
        Transform _playerTransform = ServiceLocator.Instance.GetService<PlayerLocation>().gameObject.transform;

        goToTarget = true;
        _agent.isStopped = true;
        FacePlayer(_playerTransform);
    }

    #endregion Helper Functions
}