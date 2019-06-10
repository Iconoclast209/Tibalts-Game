using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectedAction
{
    seed,
    stack,
    hold
}

public class GameController : MonoBehaviour
{

    #region FIELDS
    //Visible in Inspector
    [SerializeField]
    Text bubblesRemainingText;
    [SerializeField]
    GameObject seedButton;



    //NOT visible in inspector
    int numberOfPlayers = 2;
    int playerTurn = 1;
    public Square[] squares;
    SelectedAction currentAction = SelectedAction.hold;
    Color selectedColor = Color.yellow;
    Color player1Color = Color.green;
    Color player2Color = Color.red;
    


    #endregion

    #region PROPERTIES
    
    public SelectedAction CurrentAction
    {
        get { return currentAction; }
    }

    public int CurrentPlayerTurn
    {
        get { return playerTurn; }
    }

    public Color SelectedColor
    {
        get { return selectedColor; }
    }
    public Color ReturnCurrentPlayerColor()
    {
        if (CurrentPlayerTurn == 1)
            return player1Color;
        else
            return player2Color;
    }



    #endregion

    #region METHODS
    // Start is called before the first frame update
    void Start()
    {
        squares = FindObjectsOfType<Square>();
        Debug.Log("Found " + squares.Length + " squares.");
        Debug.Log(SelectedAction.seed);
    }

    public void SeedButtonClicked()
    {
        currentAction = SelectedAction.seed;
    }

    public void StackButtonClicked()
    {
        currentAction = SelectedAction.stack;
    }


    #endregion
}
