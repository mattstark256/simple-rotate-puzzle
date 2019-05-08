using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileSync))]
public class TileSyncEditor : TileEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TileSync tileSync = (TileSync)target;

        EditorGUI.BeginChangeCheck();
        int syncGroup = EditorGUILayout.IntField("Sync group", tileSync.GetSyncGroup());
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(tileSync, "Changed sync group");
            EditorUtility.SetDirty(tileSync);
            tileSync.SetSyncGroup(syncGroup);
        }
    }
}
