using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public struct ClickInput
{
    public Tile tile;
    public int amount;

    public ClickInput(Tile _tile, int _amount)
    {
        tile = _tile;
        amount = _amount;
    }
}



[RequireComponent(typeof(MouseRaycast), typeof(GameController))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private Text tutorialText;

    private MouseRaycast mouseRaycast;
    private GameController gameController;

    private List<ClickInput> inputBuffer = new List<ClickInput>();


    private void Awake()
    {
        mouseRaycast = GetComponent<MouseRaycast>();
        gameController = GetComponent<GameController>();
    }


    void Update()
    {
        RaycastHit hit = mouseRaycast.RaycastFromMouse();
        if (hit.collider!=null)
        {
            Tile tile = hit.collider.GetComponent<Tile>();
            if (tile!=null)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    inputBuffer.Add(new ClickInput(tile, -1));
                    tutorialText.enabled = false;
                }
                else if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    inputBuffer.Add(new ClickInput(tile, 1));
                    tutorialText.enabled = false;
                }
            }
        }

        
        int i = 0;
        while (i < inputBuffer.Count)
        {
            if (inputBuffer[i].tile.GetPuzzle().Solved() ||
                inputBuffer[i].tile.GetPuzzle() != gameController.GetCurrentPuzzle())
            {
                inputBuffer.RemoveAt(i);
            }
            else if (!inputBuffer[i].tile.IsRotating())
            {
                inputBuffer[i].tile.Rotate(inputBuffer[i].amount);
                inputBuffer.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }
}
