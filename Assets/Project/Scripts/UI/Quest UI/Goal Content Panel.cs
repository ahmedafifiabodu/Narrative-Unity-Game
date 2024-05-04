using UnityEngine;

public class GoalContentPanel : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _goalNameText;

    public void SetGoalText(string goalName, bool isActive)
    {
        if (isActive)
        {
            _goalNameText.text = $"<color=#DDC775>{goalName}</color>";
        }
        else
        {
            _goalNameText.text = goalName;
        }
    }
}