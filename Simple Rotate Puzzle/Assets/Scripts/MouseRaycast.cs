using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This can be used to find out what the mouse is over in 3D space.
// To check if the mouse has hit anything, just check if the RaycastHit collider is null.

public class MouseRaycast : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField, Tooltip("If this is null on Awake it defaults to Camera.main")]
    private Camera cam;
    [SerializeField]
    private float maxDistance = 1000f;

    private void Awake()
    {
        if (cam == null) { cam = Camera.main; }
    }

    public RaycastHit RaycastFromMouse()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, maxDistance, layerMask);
        return hit;
    }
}
