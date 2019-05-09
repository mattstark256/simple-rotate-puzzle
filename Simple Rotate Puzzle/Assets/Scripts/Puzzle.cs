using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    private bool solved = false;
    public bool Solved() { return solved; }

    private bool rotatingGroup = false;

    private Tile[] tiles;

    private Vector3 averagePosition;
    public Vector3 GetAveragePosition() { return averagePosition; }

    [SerializeField]
    private MismatchWarning warningPrefab;


    private void Awake()
    {
        tiles = GetComponentsInChildren<Tile>();
        CalculateAveragePosition();
        SetUpNeighbors();
        UpdateWarnings();
    }


    public void CheckIfSolved()
    {
        foreach (Tile tile in tiles)
        {
            if (tile.IsRotating())
            {
                solved = false;
                return;
            }
        }

        foreach (Tile tile in tiles)
        {
            if (tile.GetNeighbor(0) != null &&
                tile.GetSegment(0) != tile.GetNeighbor(0).GetSegment(2))
            {
                solved = false;
                return;
            }

            if (tile.GetNeighbor(1) != null &&
                tile.GetSegment(1) != tile.GetNeighbor(1).GetSegment(3))
            {
                solved = false;
                return;
            }
        }

        solved = true;
    }


    public void UpdateWarnings()
    {
        foreach (Tile tile in tiles)
        {
            if (tile.GetNeighbor(0) != null)
            {
                tile.GetWarning(0).SetWarningEnabled(tile.GetSegment(0) != tile.GetNeighbor(0).GetSegment(2));
            }

            if (tile.GetNeighbor(1) != null)
            {
                tile.GetWarning(1).SetWarningEnabled(tile.GetSegment(1) != tile.GetNeighbor(1).GetSegment(3));
            }
        }
    }


    public void RotateGroup(int syncGroup, int amount, TileSync callingTile)
    {
        if (rotatingGroup) return;
        rotatingGroup = true;
        foreach (Tile tile in tiles)
        {
            TileSync tileSync = tile as TileSync;
            if (tileSync != null &&
                tileSync != callingTile &&
                tileSync.GetSyncGroup() == syncGroup)
            {
                tileSync.Rotate(amount);
            }
        }
        rotatingGroup = false;
    }


    private void SetUpNeighbors()
    {
        //Vector3 neighbor1Vector = new Vector3(1, 1, 0);
        //Vector3 neighbor2Vector = new Vector3(1, -1, 0);
        foreach (Tile tile1 in tiles)
        {
            foreach (Tile tile2 in tiles)
            {
                if (tile2.transform.localPosition == tile1.transform.localPosition + Vector3.right)
                {
                    tile1.SetNeighbor(0, tile2);
                    tile2.SetNeighbor(2, tile1);
                    MismatchWarning warning = CreateWarning(tile1, tile2);
                    tile1.SetWarning(0, warning);
                    tile2.SetWarning(2, warning);
                }
                else if (tile2.transform.localPosition == tile1.transform.localPosition + Vector3.down)
                {
                    tile1.SetNeighbor(1, tile2);
                    tile2.SetNeighbor(3, tile1);
                    MismatchWarning warning = CreateWarning(tile1, tile2);
                    tile1.SetWarning(1, warning);
                    tile2.SetWarning(3, warning);
                }
            }
        }
    }


    private void CalculateAveragePosition()
    {
        Vector3 totalVector = Vector3.zero;
        foreach (Tile tile in tiles)
        {
            totalVector += tile.transform.position;
        }
        averagePosition = totalVector / tiles.Length;
    }


    private MismatchWarning CreateWarning(Tile tile1, Tile tile2)
    {
        MismatchWarning warning = Instantiate(warningPrefab);
        warning.transform.parent = transform;
        warning.transform.localPosition = (tile1.transform.localPosition + tile2.transform.localPosition) / 2;
        return warning;
    }
}
