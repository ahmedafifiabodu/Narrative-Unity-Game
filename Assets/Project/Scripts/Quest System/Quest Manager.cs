using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<QuestGiver> _quests;

    private ServiceLocator _serviceLocator;
    private UISystem _uISystem;

    public QuestGiver CurrentQuestGiver { get; internal set; } = new();

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Instance;
        _serviceLocator.RegisterServiceDontDestoryOnLoad(this);
    }

    private void Start()
    {
        _uISystem = _serviceLocator.GetService<UISystem>();

        ResetQuestsAndGoals();
    }

    internal void SetQuest(QuestNPC _questNPC)
    {
        foreach (QuestGiver _questGiver in _quests)
        {
            if (_questGiver.QuestNPC == _questNPC && _questGiver.Quest.QuestState == QuestState.NotActive)
            {
                _questNPC.Quest = _questGiver.Quest;
                _questNPC.Flowchart = _questGiver.Flowchart;
                _questNPC.Quest.QuestState = QuestState.Active;

                SetActiveGoal(_questGiver.Quest);
                QuestUI(_questNPC);

                break;
            }
        }
    }

    public void CompleteActiveGoal(QuestNPC _questNPC)
    {
        if (_questNPC.Quest != null &&
            _questNPC.Quest.ActiveGoal != null &&
            _questNPC.Quest.QuestState == QuestState.Active)
        {
            _questNPC.Quest.ActiveGoal.Complete = true;
            _questNPC.Quest.CheckGoals(_questNPC);

            SetActiveGoal(_questNPC.Quest);
            QuestUI(_questNPC);
        }
    }

    private void SetActiveGoal(Quest _quest)
    {
        foreach (Goal goal in _quest.QuestGoals)
        {
            if (!goal.Complete)
            {
                _quest.ActiveGoal = goal;
                break;
            }
        }
    }

    private void ResetQuestsAndGoals()
    {
        foreach (QuestGiver _questGiver in _quests)
        {
            _questGiver.Quest.QuestState = QuestState.NotActive;

            foreach (var goal in _questGiver.Quest.QuestGoals)
                goal.Complete = false;
        }
    }

    #region Quest UI

    private void QuestUI(QuestNPC _questNPC)
    {
        _uISystem.UpdateQuestUI(_questNPC);

        _uISystem.UpdateActiveGoal(_questNPC.Quest.QuestName, _questNPC.Quest.ActiveGoal.GoalName, true, _questNPC.Quest.QuestState);
    }

    #endregion Quest UI
}