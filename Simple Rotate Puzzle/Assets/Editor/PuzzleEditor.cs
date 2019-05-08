using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Puzzle))]
public class PuzzleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Puzzle puzzle = (Puzzle)target;

        if (GUILayout.Button("Rotate Puzzle"))
        {
            Tile[] tiles = puzzle.GetComponentsInChildren<Tile>();
            Transform[] transforms = new Transform[tiles.Length];
            for (int i = 0; i < tiles.Length; i++) { transforms[i] = tiles[i].transform; }

            // Both the tile and transform components are modified so they must be combined into an array for undo
            Object[] objectsToUndo = new Object[tiles.Length + transforms.Length];
            for (int i = 0; i < tiles.Length; i++) { objectsToUndo[i] = tiles[i]; }
            for (int i = 0; i < tiles.Length; i++) { objectsToUndo[tiles.Length + i] = transforms[i]; }

            Undo.RecordObjects(objectsToUndo, "Rotate Puzzle");
            for (int i = 0; i < tiles.Length; i++) { EditorUtility.SetDirty(objectsToUndo[i]); }

            foreach (Tile tile in tiles)
            {
                tile.transform.localPosition = new Vector3(tile.transform.localPosition.y, -tile.transform.localPosition.x, tile.transform.localPosition.z);
                tile.ShiftSegmentsCW();
            }
        }
    }
}
