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

    private int amountToRotate = 0;
    private float rotateDuration = 0.2f;

    protected Puzzle puzzle;
    public Puzzle GetPuzzle() { return puzzle; }

    // Each tile stores references to the four neighboring tiles
    private Tile[] neighbors = new Tile[4];
    public Tile GetNeighbor(int index) { return neighbors[index]; }
    public void SetNeighbor(int index, Tile neighbor) { neighbors[index] = neighbor; }

    // Each tile stores references to the four warnings that can appear on its sides
    private MismatchWarning[] warnings = new MismatchWarning[4];
    public MismatchWarning GetWarning(int index) { return warnings[index]; }
    public void SetWarning(int index, MismatchWarning warning) { warnings[index] = warning; }


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


    private void Update()
    {
        if (amountToRotate > 0 && !isRotating && !puzzle.Solved())
        {
            Rotate(-1);
            amountToRotate--;
        }
        if (amountToRotate < 0 && !isRotating && !puzzle.Solved())
        {
            Rotate(1);
            amountToRotate++;
        }
    }


    protected virtual void AssignMaterialColors()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetColor("_Color1", (segments[0]) ? Color.black : Color.white);
        spriteRenderer.material.SetColor("_Color2", (segments[1]) ? Color.black : Color.white);
        spriteRenderer.material.SetColor("_Color3", (segments[2]) ? Color.black : Color.white);
        spriteRenderer.material.SetColor("_Color4", (segments[3]) ? Color.black : Color.white);
    }


    // Preview settings in the scene editor. This is used because changing MeshRenderer materials using a custom editor wasn't working out.
    protected virtual void OnDrawGizmos()
    {
        for (int i = 0; i < 4; i++)
        {
            Gizmos.color = segments[i] ? Color.black : Color.white;
            Gizmos.DrawCube(transform.position + Quaternion.Euler(0, 0, - 45 - i * 90) * Vector3.up * 0.25f, Vector3.one * 0.3f);
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
        puzzle.UpdateWarnings();

        isRotating = true;
        float initialAngle = GetComponent<SpriteRenderer>().material.GetFloat("_RotateAngle");
        float f = 0;
        while (f < 1)
        {
            f += Time.deltaTime / rotateDuration;
            if (f > 1) f = 1;

            GetComponent<SpriteRenderer>().material.SetFloat("_RotateAngle", initialAngle + Mathf.SmoothStep(0, angle, f));

            yield return null;
        }
        isRotating = false;

        puzzle.CheckIfSolved();
    }


    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) amountToRotate++;
        if (Input.GetMouseButtonDown(1)) amountToRotate--;
        // rotating 4 is the same as rotating 0
        if (amountToRotate == 4 || amountToRotate == -4) amountToRotate = 0;
    }
}
