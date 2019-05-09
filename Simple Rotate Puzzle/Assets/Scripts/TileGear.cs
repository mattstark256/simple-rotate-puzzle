using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGear : Tile
{
    // OnlyRotateSelf is used to make it so only the first gear handles the chain reaction
    private bool onlyRotateSelf = false;
    public void OnlyRotateSelf() { onlyRotateSelf = true; }


    public override void Rotate(int amount)
    {
        if (onlyRotateSelf) base.Rotate(amount);
        else RotateConnectedGears(amount);
        onlyRotateSelf = false;
    }


    // Rotate all neighbors
    private void RotateConnectedGears(int amount)
    {
        List<TileGear> tilesToRotate = new List<TileGear>();
        List<int> amountsToRotate = new List<int>();
        List<TileGear> rotatedTiles = new List<TileGear>();

        tilesToRotate.Add(this);
        amountsToRotate.Add(amount);

        while (tilesToRotate.Count > 0)
        {
            // Rotate the tile
            tilesToRotate[0].OnlyRotateSelf();
            tilesToRotate[0].Rotate(amountsToRotate[0]);

            // Add any neighboring gear tiles yet to be rotated to the list
            for (int i = 0; i < 4; i++)
            {
                TileGear neighbor = tilesToRotate[0].GetNeighbor(i) as TileGear;
                if (neighbor != null &&
                    !rotatedTiles.Contains(neighbor) &&
                    !tilesToRotate.Contains(neighbor))
                {
                    tilesToRotate.Add(neighbor);
                    amountsToRotate.Add(-amountsToRotate[0]);
                }
            }

            // Remove the tile from the list
            rotatedTiles.Add(tilesToRotate[0]);
            tilesToRotate.RemoveAt(0);
            amountsToRotate.RemoveAt(0);
        }
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.grey;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.4f);
    }
}
