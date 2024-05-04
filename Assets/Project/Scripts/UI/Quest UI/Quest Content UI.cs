using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestContentUI : MonoBehaviour
{
    [Header("Quest Content")]
    [SerializeField] private TextMeshProUGUI _questName;

    [SerializeField] private TextMeshProUGUI _questDescription;
    [SerializeField] private TextMeshProUGUI _questLocation;
    [SerializeField] private Toggle _questToggle;

    [Header("Quest States")]
    [SerializeField] private GameObject _questToDoState;

    [SerializeField] private GameObject _questFailedState;
    [SerializeField] private GameObject _questCompletedState;

    internal QuestNPC QuestNPC { private get; set; }

    internal string QuestNameText
    { get => _questName.text; private set { _questName.text = value; } }

    private ServiceLocator _serviceLocator;
    private UISystem _uISystem;

    private void Start()
    {
        _serviceLocator = ServiceLocator.Instance;
        _uISystem = _serviceLocator.GetService<UISystem>();
    }

    private void OnEnable() => _questToggle.onValueChanged.AddListener(OnQuestToggleChanged);

    private void OnDisable()
    {
        _questToggle.onValueChanged.RemoveListener(OnQuestToggleChanged);
        _questToggle.isOn = false;
    }

    private void OnQuestToggleChanged(bool isOn)
    {
        _uISystem.SetGoalPanelContentActive(isOn);

        if (isOn)
        {
            List<string> _goalNames = new();

            foreach (var goal in QuestNPC.Quest.QuestGoals)
                _goalNames.Add(goal.GoalName);

            _uISystem.UpdateGoalUI(
            QuestNPC.Quest.QuestName,
            QuestNPC.Quest.QuestLocation,
            _goalNames,
            QuestNPC.Quest.ActiveGoal.Description,
            QuestNPC.Quest.QuestGoals.Count,
            QuestNPC.Quest.ActiveGoal.GoalName);

            _uISystem.UpdateActiveGoal(QuestNPC.Quest.QuestName, QuestNPC.Quest.ActiveGoal.GoalName, true, QuestNPC.Quest.QuestState);

            _serviceLocator.GetService<QuestManager>().CurrentQuestGiver.QuestNPC = QuestNPC;
        }
    }

    internal void SetQuestContent(string name, string description, string location, QuestState _questState)
    {
        _questName.text = name;
        _questDescription.text = description;
        _questLocation.text = location;

        SetQuestStatus(_questState);
    }

    internal void SetQuestStatus(QuestState _questState)
    {
        switch (_questState)
        {
            case QuestState.Active:
                _questToDoState.SetActive(true);
                _questFailedState.SetActive(false);
                _questCompletedState.SetActive(false);
                break;

            case QuestState.Failed:
                _questToDoState.SetActive(false);
                _questFailedState.SetActive(true);
                _questCompletedState.SetActive(false);
                break;

            case QuestState.Completed:
                _questToDoState.SetActive(false);
                _questFailedState.SetActive(false);
                _questCompletedState.SetActive(true);
                break;
        }
    }
}