using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    [Header("Interaction UI Elements")]
    [SerializeField] private GameObject _prompt;

    [SerializeField] private TextMeshProUGUI _promptText;

    [Header("Quest UI Elements")]
    [Header("Quest Panel")]
    [SerializeField] private GameObject _questPanel;

    [SerializeField] private GameObject _questPanelContent;

    [Header("Goal Panel")]
    [SerializeField] private GameObject _goalPanelContent;

    [Header("Toggle Group")]
    [SerializeField] private ToggleGroup _questToggleGroup;

    [Header("Active Goal Panel")]
    [SerializeField] private GameObject _activeGoalPanel;

    [SerializeField] private TextMeshProUGUI _activeQuestText;
    [SerializeField] private TextMeshProUGUI _activeGoalText;

    [Header("Quest Content Prefab")]
    [SerializeField] private GameObject _questContentPrefab;

    private ServiceLocator _serviceLocator;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Instance;
        _serviceLocator.RegisterServiceDontDestoryOnLoad(this);
    }

    private void Start()
    {
        UpdateActiveGoal("", "", false, QuestState.Completed);
        SetGoalPanelContentActive(false);
    }

    #region Interaction UI

    internal void UpdatePromptText(string _promptMessage)
    {
        _prompt.SetActive(true);
        _promptText.text = _promptMessage;
    }

    internal void DisablePromptText() => _prompt.SetActive(false);

    #endregion Interaction UI

    #region Quest UI

    internal void ToggleQuestPanel()
    {
        _questPanel.SetActive(!_questPanel.activeSelf);

        if (!_questPanel.activeSelf)
            SetGoalPanelContentActive(false);
    }

    internal void SetGoalPanelContentActive(bool _setActive) => _goalPanelContent.SetActive(_setActive);

    internal void UpdateQuestUI(QuestNPC _questNPC)
    {
        QuestContentUI existingQuestContentUI = null;

        foreach (Transform child in _questPanelContent.transform)
        {
            if (child.TryGetComponent(out QuestContentUI questContentUI) && questContentUI.QuestNameText == _questNPC.Quest.QuestName)
            {
                existingQuestContentUI = questContentUI;
                break;
            }
        }

        if (existingQuestContentUI == null)
        {
            GameObject newQuestContent = Instantiate(_questContentPrefab, _questPanelContent.transform);

            if (newQuestContent.TryGetComponent<Toggle>(out var toggle))
                toggle.group = _questToggleGroup;

            if (newQuestContent.TryGetComponent(out QuestContentUI newQuestContentUI))
            {
                existingQuestContentUI = newQuestContentUI;
                newQuestContentUI.QuestNPC = _questNPC;
            }
        }

        if (existingQuestContentUI != null)
            existingQuestContentUI.SetQuestContent(_questNPC.Quest.QuestName, _questNPC.Quest.QuestDescription, _questNPC.Quest.QuestLocation, _questNPC.Quest.QuestState);
    }

    internal void UpdateGoalUI(string _questName, string _questLocation, List<string> _goalNames, string _goalDescription, int _goalCounts, string activeGoalName)
    {
        if (_goalPanelContent.TryGetComponent(out GoalContentUI goalContentUI))
            goalContentUI.SetGoalContent(_questName, _questLocation, _goalNames, _goalDescription, _goalCounts, activeGoalName);
    }

    internal void UpdateActiveGoal(string _activeQuest, string _activeGoal, bool _setActiveGoalPanel, QuestState _questState)
    {
        if (_questState == QuestState.Active)
        {
            _activeGoalPanel.SetActive(_setActiveGoalPanel);
            _activeQuestText.text = _activeQuest;
            _activeGoalText.text = _activeGoal;
        }
        else
            _activeGoalPanel.SetActive(false);
    }

    #endregion Quest UI
}