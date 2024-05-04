using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Scriptable Object/Quest System/Quest")]
public class Quest : ScriptableObject
{
    [Header("Quest Details")]
    [SerializeField] private string _questName;

    [SerializeField][TextArea] private string _questDescription;
    [SerializeField] private string _questLocation;
    [SerializeField] private QuestState _questState;

    [Header("Quest Goals")]
    [SerializeField] private List<Goal> _goals;

    private Goal _activeGoal;

    public Goal ActiveGoal
    { get => _activeGoal; internal set { _activeGoal = value; } }

    public string QuestName
    { get => _questName; set { _questName = value; } }

    public string QuestDescription
    { get => _questDescription; set { _questDescription = value; } }

    public string QuestLocation
    { get => _questLocation; set { _questLocation = value; } }

    public QuestState QuestState
    { get => _questState; set { _questState = value; } }

    public List<Goal> QuestGoals
    { get => _goals; set { _goals = value; } }

    internal void CheckGoals(QuestNPC _questNPC)
    {
        //_isQuestCompleted = _goals.TrueForAll(goal => goal.Complete);

        if (_goals.All(goal => goal.Complete))
            QuestState = QuestState.Completed;

        if (QuestState == QuestState.Completed)
            GiveReward(_questNPC);
    }

    private void GiveReward(QuestNPC _questNPC)
    {
        //Inventory.Instance.AddItem(ItemReward);

        ServiceLocator.Instance.GetService<UISystem>().UpdateActiveGoal("", "", false, QuestState.Completed);

        _questNPC.Flowchart.SetBooleanVariable("IsQuestComplete", true);
    }
}