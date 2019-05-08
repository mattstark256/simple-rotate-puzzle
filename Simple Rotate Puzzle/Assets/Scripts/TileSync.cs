using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSync : Tile
{
    [SerializeField]
    private int syncGroup;
    public int GetSyncGroup() { return syncGroup; }
    public void SetSyncGroup(int _syncGroup) { syncGroup = _syncGroup; }

    private Color[] groupColors = {
        new Color(1f, 0.5f, 0),
        new Color(0.2f, 0.3f, 1),
        new Color(0, 0.9f, 0.1f) };
    public Color GetGroupColor()
    {
        if (syncGroup < groupColors.Length) return groupColors[syncGroup];
        else return Color.grey;
    }

    [SerializeField]
    private MeshRenderer symbolRenderer;
    public MeshRenderer GetSymbolRenderer() { return symbolRenderer; }
    public void SetSymbolRenderer(MeshRenderer _symbolRenderer) { symbolRenderer = _symbolRenderer; }



    public override void Rotate(int amount)
    {
        puzzle.RotateGroup(syncGroup, amount, this);

        base.Rotate(amount);
    }


    protected override void AssignMaterialColors()
    {
        base.AssignMaterialColors();

        symbolRenderer.material.color = GetGroupColor();
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = GetGroupColor();
        Gizmos.DrawCube(transform.position, Vector3.one * 0.6f);
    }
}
