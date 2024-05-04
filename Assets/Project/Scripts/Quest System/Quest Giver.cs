using Fungus;
using UnityEngine;

[System.Serializable]
public class QuestGiver
{
    [SerializeField] private Quest _quest;
    [SerializeField] private QuestNPC _questNPC;
    [SerializeField] private Flowchart _flowchart;

    public Quest Quest
    { get => _quest; set { _quest = value; } }

    public QuestNPC QuestNPC
    { get => _questNPC; set { _questNPC = value; } }

    public Flowchart Flowchart
    { get => _flowchart; set { _flowchart = value; } }
}