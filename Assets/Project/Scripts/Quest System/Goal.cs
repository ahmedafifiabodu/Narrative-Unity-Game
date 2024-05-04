using UnityEngine;

[CreateAssetMenu(fileName = "New Goal", menuName = "Scriptable Object/Quest System/Goal")]
public class Goal : ScriptableObject
{
    [SerializeField] private string _goalName;
    [SerializeField][TextArea] private string _description;
    [SerializeField] internal bool Complete;

    public string GoalName
    { get => _goalName; set { _goalName = value; } }

    public string Description
    { get => _description; set { _description = value; } }
}