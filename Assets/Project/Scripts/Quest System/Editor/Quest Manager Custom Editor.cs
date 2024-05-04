using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestManager))]
public class QuestManagerCustomEditor : Editor
{
    private void OnEnable() => EditorApplication.update += Repaint;

    private void OnDisable() => EditorApplication.update -= Repaint;

    public override void OnInspectorGUI()
    {
        QuestManager questManager = (QuestManager)target;

        // Draw the default inspector
        DrawDefaultInspector();

        if (EditorApplication.isPlaying)
        {
            if (questManager.CurrentQuestGiver == null || questManager.CurrentQuestGiver.QuestNPC.Quest == null)
            {
                EditorGUILayout.HelpBox("There is no active quest.", MessageType.Error);
            }
            else
            {
                EditorGUILayout.HelpBox("There is an active quest.", MessageType.Info);

                questManager.CurrentQuestGiver.QuestNPC.Quest = (Quest)EditorGUILayout.ObjectField("Current Quest Reference", questManager.CurrentQuestGiver.Quest, typeof(Quest), true);

                GUIStyle _questGiverName = new(GUI.skin.label) { normal = { textColor = Color.green } };
                EditorGUILayout.LabelField("NPC Name: " + questManager.CurrentQuestGiver.QuestNPC.Quest.name, _questGiverName);

                GUIStyle _currentQuestName = new(GUI.skin.label) { normal = { textColor = Color.green } };
                EditorGUILayout.LabelField("Current Quest: " + questManager.CurrentQuestGiver.Quest.QuestName, _currentQuestName);

                GUIStyle _questStatus = new(GUI.skin.label) { normal = { textColor = Color.blue } };
                EditorGUILayout.LabelField("Quest Status: " + questManager.CurrentQuestGiver.Quest.QuestState.ToString(), _questStatus);

                if (questManager.CurrentQuestGiver.Quest.ActiveGoal != null)
                {
                    GUIStyle _currentGoalName = new(GUI.skin.label) { normal = { textColor = Color.yellow } };
                    EditorGUILayout.LabelField("Current Objective: " + questManager.CurrentQuestGiver.Quest.ActiveGoal.GoalName, _currentGoalName);
                }
            }
        }
        else
            EditorGUILayout.HelpBox("Please Run the game to show the active quest.", MessageType.Warning);
    }
}