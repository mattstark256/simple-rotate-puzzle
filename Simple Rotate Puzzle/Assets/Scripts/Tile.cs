using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private bool[] segments = new bool[4];
    public bool GetSegment(int index) { return segments[index]; }
    public void SetSegment(int index, bool value) { segments[index] = value; }

    private bool isRotating = false;
    public bool IsRotating() { return isRotating; }

    private float rotateDuration = 0.2f;

    protected Puzzle puzzle;
    public Puzzle GetPuzzle() { return puzzle; }

    // Each tile stores references to the four neighboring tiles
    private Tile[] neighbors = new Tile[4];
    public Tile GetNeighbor(int index) { return neighbors[index]; }
    public void SetNeighbor(int index, Tile neighbor) { neighbors[index] = neighbor; }


    public void ShiftSegmentsCW()
    {
        bool storedSegment = segments[3];
        for (int i = 3; i > 0; i--)
        { segments[i] = segments[i - 1]; }
        segments[0] = storedSegment;
    }


    public void ShiftSegmentsCCW()
    {
        bool storedSegment = segments[0];
        for (int i = 0; i < 3; i++)
        { segments[i] = segments[i + 1]; }
        segments[3] = storedSegment;
    }


    private void Awake()
    {
        AssignMaterialColors();
    }


    private void Start()
    {
        puzzle = GetComponentInParent<Puzzle>();
        if (!puzzle) Debug.Log("No puzzle component found in parents!");
    }


    protected virtual void AssignMaterialColors()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.SetColor("_Color1", (segments[0]) ? Color.black : Color.white);
        meshRenderer.material.SetColor("_Color2", (segments[1]) ? Color.black : Color.white);
        meshRenderer.material.SetColor("_Color3", (segments[2]) ? Color.black : Color.white);
        meshRenderer.material.SetColor("_Color4", (segments[3]) ? Color.black : Color.white);

        //for (int i = 0; i < 4; i++)
        //{
        //    meshRenderer.materials[i].color = segments[i] ? Color.black : Color.white;
        //}
    }


    // Preview settings in the scene editor. This is used because changing MeshRenderer materials using a custom editor wasn't working out.
    protected virtual void OnDrawGizmos()
    {
        for (int i = 0; i < 4; i++)
        {
            Gizmos.color = segments[i] ? Color.black : Color.white;
            Gizmos.DrawCube(transform.position + Quaternion.Euler(0, 0, -45 - i * 90) * Vector3.up * 0.4f, Vector3.one * 0.5f);
        }
    }


    public virtual void Rotate(int amount)
    {
        if (amount == 1)
        {
            ShiftSegmentsCW();
        }
        if (amount == -1)
        {
            ShiftSegmentsCCW();
        }

        StartCoroutine(RotateCoroutine(90 * amount));
    }


    private IEnumerator RotateCoroutine(float angle)
    {
        isRotating = true;
        //Quaternion startRotation = transform.localRotation;
        float initialAngle = GetComponent<MeshRenderer>().material.GetFloat("_RotateAngle");
        float f = 0;
        while (f < 1)
        {
            f += Time.deltaTime / rotateDuration;
            if (f > 1) f = 1;

            GetComponent<MeshRenderer>().material.SetFloat("_RotateAngle", initialAngle + Mathf.SmoothStep(0, angle, f));
            //transform.localRotation = startRotation * Quaternion.Euler(0, 0, Mathf.SmoothStep(0, angle, f));

            yield return null;
        }
        isRotating = false;

        // Check for solved
        puzzle.CheckIfSolved();
    }
}
