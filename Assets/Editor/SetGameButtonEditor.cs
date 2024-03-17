using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SetGameSetting))]
public class SetGameButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SetGameSetting myscript = target as SetGameSetting;

        switch (myscript.buttonType)
        {
            case SetGameSetting.EButtonType.PairNumberBtn:
                myscript.PairNumber = (GameSettings.EPairNumber)EditorGUILayout.EnumPopup("Pair Number", myscript.PairNumber);
                break;
            case SetGameSetting.EButtonType.PuzzleCategoryBtn:
                myscript.PuzlleCategories = (GameSettings.EPuzlleCategories)EditorGUILayout.EnumPopup("Puzzle Categories", myscript.PuzlleCategories);
                break;

        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
