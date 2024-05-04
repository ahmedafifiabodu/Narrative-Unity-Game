using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Material _outLineMaterial;
    [SerializeField] private LayerMask _interactableLayerMask;

    private ServiceLocator serviceLocator;
    private UISystem _playerUI;
    private InputManager _inputManager;
    private AudioManager _audioManager;

    private Interactable currentInteractable;

    private bool hasPlayedInteractSFX = false;

    private void Start()
    {
        serviceLocator = ServiceLocator.Instance;

        _playerUI = serviceLocator.GetService<UISystem>();
        _inputManager = serviceLocator.GetService<InputManager>();
        _audioManager = serviceLocator.GetService<AudioManager>();

        _playerUI.DisablePromptText();
    }

    private void Update()
    {
        if (currentInteractable != null)
        {
            if (currentInteractable.AutoInteract)
            {
                if (!hasPlayedInteractSFX)
                {
                    _audioManager.PlayWalkSFX(_audioManager._interact);
                    hasPlayedInteractSFX = true;
                }

                currentInteractable.BaseInteract();
            }
            else
            {
                if (_inputManager.PlayerActions.Interact.triggered)
                {
                    if (!hasPlayedInteractSFX)
                    {
                        _audioManager.PlayWalkSFX(_audioManager._interact);
                        hasPlayedInteractSFX = true;
                    }

                    currentInteractable.BaseInteract();
                }
            }
        }
        else
            hasPlayedInteractSFX = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _interactableLayerMask) != 0)
        {
            if (other.TryGetComponent<Interactable>(out var _interactable))
            {
                Renderer[] renderers = other.GetComponentsInChildren<Renderer>();

                foreach (Renderer renderer in renderers)
                {
                    if (renderer != null)
                    {
                        _interactable.OriginalMaterials = renderer.materials;

                        Material[] materialsWithOutline = new Material[renderer.materials.Length + 1];
                        renderer.materials.CopyTo(materialsWithOutline, 0);
                        materialsWithOutline[^1] = _outLineMaterial;

                        renderer.materials = materialsWithOutline;
                    }
                }
            }

            if (!_interactable.AutoInteract)
            {
                _playerUI.UpdatePromptText(_interactable.PromptMessage);
                currentInteractable = _interactable;
            }
            else
                currentInteractable = _interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & _interactableLayerMask) != 0)
            SetCurrentInteractableToNull();
    }

    public void SetCurrentInteractableToNull()
    {
        if (currentInteractable == null)
            return;

        Renderer[] renderers = currentInteractable.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
                renderer.materials = currentInteractable.OriginalMaterials;
        }

        _playerUI.DisablePromptText();
        currentInteractable = null;
    }
}