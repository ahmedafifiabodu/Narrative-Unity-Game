using Fungus;
using UnityEngine;

public class QuestNPC : MonoBehaviour
{
    public Quest Quest { get; set; }
    internal Flowchart Flowchart { get; set; }

    private ServiceLocator _serviceLocator;

    private bool _assignedQuest;
    private bool _helped;

    private void Start() => _serviceLocator = ServiceLocator.Instance;

    public void Interact()
    {
        if (!_assignedQuest && !_helped)
        {
            _serviceLocator.GetService<QuestManager>().SetQuest(this);
            AssignQuest(true);
        }
        else if (_assignedQuest && !_helped)
        {
            CheckQuest();
        }
        else
        {
            // Open Quest Dialog
        }
    }

    private void AssignQuest(bool _doIAssignQuest)
    {
        Flowchart.SetBooleanVariable(GameConstant.IsQuestActive, true);

        _assignedQuest = _doIAssignQuest;

        if (_doIAssignQuest)
            Quest.QuestState = QuestState.Active;
    }

    private void CheckQuest()
    {
        if (Quest.QuestState == QuestState.Completed)
        {
            //Say Quest Complete

            Flowchart.SetBooleanVariable(GameConstant.IsQuestActive, false);
            Flowchart.SetBooleanVariable(GameConstant.IsQuestComplete, true);

            _helped = true;
            AssignQuest(false);
        }
        else
        {
            // Say Quest Not Complete
        }
    }
}