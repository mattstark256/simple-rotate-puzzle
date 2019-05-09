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
        new Color(0.25f, 0.4f, 1),
        new Color(0, 0.85f, 0.1f) };
    public Color GetGroupColor()
    {
        if (syncGroup < groupColors.Length) return groupColors[syncGroup];
        else return Color.grey;
    }


    public override void Rotate(int amount)
    {
        puzzle.RotateGroup(syncGroup, amount, this);

        base.Rotate(amount);
    }


    protected override void AssignMaterialColors()
    {
        base.AssignMaterialColors();
        
        GetComponent<SpriteRenderer>().material.SetColor("_SymbolColor", GetGroupColor());
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = GetGroupColor();
        Gizmos.DrawCube(transform.position, Vector3.one * 0.4f);
    }
}
