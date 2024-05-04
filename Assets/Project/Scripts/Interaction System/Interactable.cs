using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableLayerMask = 6;
    [SerializeField] private bool _autoInteract = false;
    [SerializeField] private bool _useEvents;
    [SerializeField] private string _promptMessage;

    private int? _pendingLayerChange = null;

    public bool AutoInteract
    { get => _autoInteract; set { _autoInteract = value; } }

    public string PromptMessage
    { get => _promptMessage; set { _promptMessage = value; } }

    public bool UseEvents
    { get => _useEvents; set { _useEvents = value; } }

    public Material[] OriginalMaterials { get; set; }

    protected virtual string OnLook() => _promptMessage;

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            if (TryGetComponent<BoxCollider>(out var boxCollider))
                boxCollider.isTrigger = true;

            if (gameObject.activeInHierarchy)
                StartCoroutine(DelayedLayerChange());
            else
                _pendingLayerChange = _interactableLayerMask;
        }
    }

    private void OnEnable()
    {
        if (_pendingLayerChange.HasValue)
        {
            gameObject.layer = _pendingLayerChange.Value;
            _pendingLayerChange = null;
        }
        else
            gameObject.layer = _interactableLayerMask;
    }

    private IEnumerator DelayedLayerChange()
    {
        yield return null;
        gameObject.layer = _interactableLayerMask;
    }

    internal void BaseInteract()
    {
        if (_useEvents)
        {
            if (gameObject.TryGetComponent<InteractableEvents>(out var _events))
                _events.onInteract.Invoke();
        }
        else
            Interact();
    }

    protected virtual void Interact() => Debug.Log($"(Virtual) Interacting with {gameObject.name}");
}