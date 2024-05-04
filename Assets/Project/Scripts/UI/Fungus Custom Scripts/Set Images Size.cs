using UnityEngine;
using UnityEngine.UI;

public class SetImagesSize : MonoBehaviour
{
    [SerializeField] private RectTransform targetRectTransform;

    private void Start()
    {
        // Force an immediate update of the layout
        LayoutRebuilder.ForceRebuildLayoutImmediate(targetRectTransform);

        RectTransform rectTransform = GetComponent<RectTransform>();

        // Now the sizeDelta should be updated
        rectTransform.sizeDelta = targetRectTransform.sizeDelta;
    }
}