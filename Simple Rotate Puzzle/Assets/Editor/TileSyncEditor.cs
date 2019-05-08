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

        EditorGUI.BeginChangeCheck();
        MeshRenderer symbolRenderer = (MeshRenderer)EditorGUILayout.ObjectField("Symbol renderer", tileSync.GetSymbolRenderer(), typeof(MeshRenderer), true);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(tileSync, "Changed symbol renderer");
            EditorUtility.SetDirty(tileSync);
            tileSync.SetSymbolRenderer(symbolRenderer);
        }

        // Idk how to position this properly
        //EditorGUIUtility.DrawColorSwatch(new Rect(100, 100, 50, 50), tileSync.GetGroupColor());
    }
}
