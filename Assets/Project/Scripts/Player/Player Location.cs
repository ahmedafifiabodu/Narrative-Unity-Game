using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    private void Awake() => ServiceLocator.Instance.RegisterServiceDontDestoryOnLoad(this);
}