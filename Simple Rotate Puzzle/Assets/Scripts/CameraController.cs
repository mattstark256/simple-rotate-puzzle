using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GameController))]
public class CameraController : MonoBehaviour
{
    private GameController gameController;

    [SerializeField, Tooltip("If this is null on Awake it defaults to Camera.main")]
    private Camera cam;
    [SerializeField]
    private Vector3 cameraPositionOffset = Vector3.back * 10;
    [SerializeField]
    private float cameraMoveSpeed = 7;


    private void Awake()
    {
        gameController = GetComponent<GameController>();

        if (cam == null) { cam = Camera.main; }
    }


    private void Start()
    {
        cam.transform.position = gameController.GetCurrentPuzzle().GetAveragePosition() + cameraPositionOffset;
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = gameController.GetCurrentPuzzle().GetAveragePosition() + cameraPositionOffset;
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, Time.deltaTime * cameraMoveSpeed);
    }
}
