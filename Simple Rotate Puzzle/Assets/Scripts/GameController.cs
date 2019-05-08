using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    [SerializeField]
    private int currentPuzzleIndex = 0;
    [SerializeField]
    private Text continueText;
    [SerializeField]
    private string winSceneName;
    [SerializeField]
    private Puzzle[] puzzleSequence;

    public Puzzle GetCurrentPuzzle(){ return puzzleSequence[currentPuzzleIndex]; }

    private bool awaitingContinue = false;


    private void Start()
    {
        continueText.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (!awaitingContinue && puzzleSequence[currentPuzzleIndex].Solved())
        {
            awaitingContinue = true;
            continueText.enabled = true;
        }

        if (awaitingContinue && Input.GetKeyDown(KeyCode.Space))
        {
            awaitingContinue = false;
            continueText.enabled = false;

            if (currentPuzzleIndex == puzzleSequence.Length - 1)
            {
                SceneManager.LoadScene(winSceneName);
            }
            else
            {
                currentPuzzleIndex++;
            }
        }
    }


    private void OnDrawGizmos()
    {
        // Draw a line showing the sequence of puzzles
        Gizmos.color = Color.cyan;
        for (int i = 0; i < puzzleSequence.Length-1; i++)
        {
            if (puzzleSequence[i]!=null && puzzleSequence[i+1]!=null)
            {
                Gizmos.DrawLine(puzzleSequence[i].transform.position, puzzleSequence[i + 1].transform.position);
            }
        }
    }
}
