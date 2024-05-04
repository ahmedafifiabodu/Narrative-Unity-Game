using Fungus;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Flowchart _flowchart;
    [SerializeField] private LayerMask _obstacleMask;
    private Vector3 _startPosition;
    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _startPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_obstacleMask.value & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            _flowchart.ExecuteBlock(GameConstant.Death);

            _characterController.enabled = false;
            transform.position = _startPosition;
            _characterController.enabled = true;
        }
    }
}