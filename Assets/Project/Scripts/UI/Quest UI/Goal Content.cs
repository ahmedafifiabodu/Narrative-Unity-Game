using System.Collections.Generic;
using UnityEngine;

public class GoalContentUI : MonoBehaviour
{
    [Header("Quest Content")]
    [SerializeField] private TMPro.TextMeshProUGUI _questName;

    [SerializeField] private TMPro.TextMeshProUGUI _questLocation;

    [Header("Goal Content")]
    [SerializeField] private TMPro.TextMeshProUGUI _goalDescription;

    [SerializeField] private GameObject _goalContentPrefab;

    [SerializeField] private GameObject _goalParentLocationContent;

    internal void SetGoalContent(string _questName, string _questLocation, List<string> _goalNames, string _goalDescription, int _goalCounts, string activeGoalName)
    {
        ClearGoalContent();

        this._questName.text = _questName;
        this._questLocation.text = _questLocation;
        this._goalDescription.text = _goalDescription;

        CreateGoalContent(_goalCounts, _goalNames, activeGoalName);
    }

    private void CreateGoalContent(int _goalContentCount, List<string> _goalNames, string activeGoalName)
    {
        for (int i = 0; i < _goalContentCount; i++)
        {
            GameObject newQuestContent = Instantiate(_goalContentPrefab, _goalParentLocationContent.transform);

            if (newQuestContent.TryGetComponent(out GoalContentPanel goalContentPanel))
            {
                bool isActive = _goalNames[i] == activeGoalName;
                goalContentPanel.SetGoalText(_goalNames[i], isActive);
            }
        }
    }

    private void ClearGoalContent()
    {
        foreach (Transform child in _goalParentLocationContent.transform)
        {
            if (child.gameObject.TryGetComponent(out GoalContentPanel _))
                Destroy(child.gameObject);
        }
    }
}