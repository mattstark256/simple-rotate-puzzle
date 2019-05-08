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
    private GameObject mismatchErrorPrefab;
    private List<GameObject> mismatchErrors = new List<GameObject>();


    private void Awake()
    {
        tiles = GetComponentsInChildren<Tile>();
        CalculateAveragePosition();
        SetUpNeighbors();
        CheckIfSolved();
    }


    public void CheckIfSolved()
    {
        while (mismatchErrors.Count > 0)
        {
            Destroy(mismatchErrors[0].gameObject);
            mismatchErrors.RemoveAt(0);
        }

        solved = true;
        foreach (Tile tile in tiles)
        {
            if (tile.IsRotating())
            {
                solved = false;
            }
            else
            {
                if (tile.GetNeighbor(0) != null &&
                    !tile.GetNeighbor(0).IsRotating() &&
                    tile.GetSegment(0) != tile.GetNeighbor(0).GetSegment(2))
                {
                    solved = false;
                    CreateErrorObject(tile, tile.GetNeighbor(0));
                }

                if (tile.GetNeighbor(1) != null &&
                    !tile.GetNeighbor(1).IsRotating() &&
                    tile.GetSegment(1) != tile.GetNeighbor(1).GetSegment(3))
                {
                    solved = false;
                    CreateErrorObject(tile, tile.GetNeighbor(1));
                }
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
        Vector3 neighbor1Vector = new Vector3(1, 1, 0);
        Vector3 neighbor2Vector = new Vector3(1, -1, 0);
        foreach (Tile tile1 in tiles)
        {
            foreach (Tile tile2 in tiles)
            {
                if (tile2.transform.localPosition == tile1.transform.localPosition + neighbor1Vector)
                {
                    tile1.SetNeighbor(0, tile2);
                    tile2.SetNeighbor(2, tile1);
                }
                else if (tile2.transform.localPosition == tile1.transform.localPosition + neighbor2Vector)
                {
                    tile1.SetNeighbor(1, tile2);
                    tile2.SetNeighbor(3, tile1);
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


    private void CreateErrorObject(Tile tile1, Tile tile2)
    {
        GameObject newErrorObject = Instantiate(mismatchErrorPrefab);
        newErrorObject.transform.parent = transform;
        newErrorObject.transform.localPosition = (tile1.transform.localPosition + tile2.transform.localPosition) / 2;
        mismatchErrors.Add(newErrorObject);
    }
}
