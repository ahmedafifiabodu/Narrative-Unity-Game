using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Quest))]
public class QuestCustomEditor : Editor
{
    private bool isAddGoalPanelOpen = false;

    private string newGoalAssetName = "";
    private string newGoalName = "";
    private string newGoalDescription = "";

    private bool isEditGoalPanelOpen = false;
    private string goalToEditAssetName = "";
    private Goal goalToEdit = null;

    private bool isRemoveGoalPanelOpen = false;
    private string goalToRemoveName = "";

    public override void OnInspectorGUI()
    {
        SerializedObject serializedQuest = serializedObject;
        serializedQuest.Update();

        Quest quest = (Quest)target;

        quest.QuestName = EditorGUILayout.TextField("Quest Name", quest.QuestName);
        quest.QuestLocation = EditorGUILayout.TextField("Quest Location", quest.QuestLocation);
        quest.QuestDescription = EditorGUILayout.TextArea(quest.QuestDescription, GUILayout.Height(100));
        quest.QuestState = (QuestState)EditorGUILayout.EnumPopup("Quest State", quest.QuestState);

        SerializedProperty goalsProperty = serializedQuest.FindProperty("_goals");
        EditorGUILayout.PropertyField(goalsProperty, true);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Add Goal"))
            isAddGoalPanelOpen = true;

        if (isAddGoalPanelOpen)
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            newGoalAssetName = EditorGUILayout.TextField("Asset Name", newGoalAssetName);
            newGoalName = EditorGUILayout.TextField("Goal Name", newGoalName);
            newGoalDescription = EditorGUILayout.TextField("Description", newGoalDescription);

            if (GUILayout.Button("Create Goal"))
            {
                Goal newGoal = CreateInstance<Goal>();

                newGoal.GoalName = newGoalName;
                newGoal.Description = newGoalDescription;
                quest.QuestGoals.Add(newGoal);

                string questPath = AssetDatabase.GetAssetPath(quest);
                string directory = Path.GetDirectoryName(questPath);

                AssetDatabase.CreateAsset(newGoal, Path.Combine(directory, newGoalAssetName + ".asset"));
                AssetDatabase.SaveAssets();

                isAddGoalPanelOpen = false;
                newGoalName = "";
                newGoalDescription = "";
                newGoalAssetName = "";
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Edit Goal"))
            isEditGoalPanelOpen = true;

        if (isEditGoalPanelOpen)
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            goalToEditAssetName = EditorGUILayout.TextField("Goal Asset Name", goalToEditAssetName);

            goalToEdit = quest.QuestGoals.FirstOrDefault(goal => Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(goal)) == goalToEditAssetName);

            if (goalToEdit == null)
                EditorGUILayout.HelpBox("No goal found with the asset name " + goalToEditAssetName, MessageType.Error);
            else
            {
                goalToEdit.GoalName = EditorGUILayout.TextField("Goal Name", goalToEdit.GoalName);
                goalToEdit.Description = EditorGUILayout.TextField("Description", goalToEdit.Description);

                if (GUILayout.Button("Save Changes"))
                {
                    EditorUtility.SetDirty(goalToEdit);
                    AssetDatabase.SaveAssets();
                    isEditGoalPanelOpen = false;
                    goalToEditAssetName = "";
                    goalToEdit = null;
                }
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Remove Goal"))
            isRemoveGoalPanelOpen = true;

        if (isRemoveGoalPanelOpen)
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            goalToRemoveName = EditorGUILayout.TextField("Goal Name", goalToRemoveName);

            Goal goalToRemove = quest.QuestGoals.FirstOrDefault(goal => Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(goal)) == goalToRemoveName);

            if (goalToRemove == null)
                EditorGUILayout.HelpBox("No goal found with the asset name " + goalToRemoveName, MessageType.Error);
            else
            {
                if (GUILayout.Button("Confirm Remove"))
                {
                    quest.QuestGoals.Remove(goalToRemove);
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(goalToRemove));
                    AssetDatabase.SaveAssets();
                    isRemoveGoalPanelOpen = false;
                    goalToRemoveName = "";
                }
            }
        }

        serializedQuest.ApplyModifiedProperties();
    }
}