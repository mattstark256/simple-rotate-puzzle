using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tile))]
public class TileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        Tile tile = (Tile)target;

        GUILayout.Label("Tile segments");
        GUILayout.BeginHorizontal();
        ShowSegmentToggle(tile, 3);
        ShowSegmentToggle(tile, 0);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        ShowSegmentToggle(tile, 2);
        ShowSegmentToggle(tile, 1);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Rotate CCW"))
        {
            Undo.RecordObject(tile, "Rotate CCW");
            EditorUtility.SetDirty(tile);
            tile.ShiftSegmentsCCW();
        }
        if (GUILayout.Button("Rotate CW"))
        {
            Undo.RecordObject(tile, "Rotate CW");
            EditorUtility.SetDirty(tile);
            tile.ShiftSegmentsCW();
        }
        GUILayout.EndHorizontal();
    }

    private void ShowSegmentToggle(Tile tile, int i)
    {
        EditorGUI.BeginChangeCheck();
        bool newValue = EditorGUILayout.Toggle(tile.GetSegment(i), GUILayout.MaxWidth(15));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(tile, "Toggle segment");
            EditorUtility.SetDirty(tile);
            tile.SetSegment(i, newValue);
        }
    }
}
