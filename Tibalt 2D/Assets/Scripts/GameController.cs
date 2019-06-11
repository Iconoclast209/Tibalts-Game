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
    Text currentPlayerText;
    [SerializeField]
    GameObject seedButton;
    [SerializeField]
    GameObject stackButton;
    [SerializeField]
    GameObject progressButton;
    [SerializeField]
    GameObject endTurnButton;
    [SerializeField]
    Sprite seedSpriteSelected;
    [SerializeField]
    Sprite seedSpriteUnSelected;
    [SerializeField]
    Sprite stackSpriteSelected;
    [SerializeField]
    Sprite stackSpriteUnSelected;
    [SerializeField]
    Sprite progressSpriteSelected;
    [SerializeField]
    Sprite progressSpriteUnSelected;
    [SerializeField]
    Sprite endTurnSpriteSelected;
    [SerializeField]
    Sprite endTurnSpriteUnSelected;



    //NOT visible in inspector
    int numberOfPlayers = 2;
    int playerTurn = 1;
    int bubblesRemaining = 0;
    public Square[] squares;
    SelectedAction currentAction = SelectedAction.hold;
    Color selectedColor = Color.yellow;
    Color player1Color = Color.green;
    Color player2Color = Color.red;
    Image seedButtonImage;
    Image stackButtonImage;
    Image progressButtonImage;
    Image endTurnButtonImage;
    Dictionary<Vector2, Square> squareDictionary = new Dictionary<Vector2, Square>();
    


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

    #endregion

    #region METHODS

    void Start()
    {
        //Create an array of all the squares on the playing field.
        squares = FindObjectsOfType<Square>();
        Debug.Log("Found " + squares.Length + " squares.");
        //Store References to the button images (to change when clicked)
        seedButtonImage = seedButton.GetComponent<Image>();
        stackButtonImage = stackButton.GetComponent<Image>();
        progressButtonImage = progressButton.GetComponent<Image>();
        endTurnButtonImage = endTurnButton.GetComponent<Image>();
        //Update the current player and bubble display
        UpdatePlayerAndBubbleDisplay();
        //Build a dictionary of all the squares 
        AddSquaresToDictionary();
    }

    //This method will add all of the squares to the dictionary 
    //which are accessible by their positions relative to the bottom 
    //left corner of the board.
    void AddSquaresToDictionary()
    {
        foreach (Square sq in squares)
        {
            squareDictionary.Add(sq.Location, sq);
        }
    }


    //This method will return the current player's color.
    public Color ReturnCurrentPlayerColor()
    {
        if (CurrentPlayerTurn == 1)
            return player1Color;
        else
            return player2Color;
    }

    //This happens when you click the SEED button.
    public void SeedButtonClicked()
    {
        currentAction = SelectedAction.seed;
        UnSelectAllButtons();
        UnSelectAllSquares();
        seedButtonImage.sprite = seedSpriteSelected;
    }

    //This happens when you click the STACK button.
    public void StackButtonClicked()
    {
        currentAction = SelectedAction.stack;
        UnSelectAllButtons();
        UnSelectAllSquares();
        stackButtonImage.sprite = stackSpriteSelected;
    }
    
    //This happens when you click the PROGRESS button.
    public void ProgressButtonClicked()
    {
        currentAction = SelectedAction.hold;
        UnSelectAllButtons();
        UnSelectAllSquares();
        progressButtonImage.sprite = progressSpriteSelected;
    }

    //This happens when you click the END TURN button.
    public void EndTurnButtonClicked()
    {
        currentAction = SelectedAction.hold;
        UnSelectAllButtons();
        UnSelectAllSquares();
        endTurnButtonImage.sprite = endTurnSpriteSelected;
    }


    //This method will clear all selections from the buttons.
    void UnSelectAllButtons()
    {
        seedButtonImage.sprite = seedSpriteUnSelected;
        stackButtonImage.sprite = stackSpriteUnSelected;
        progressButtonImage.sprite = progressSpriteUnSelected;
        endTurnButtonImage.sprite = endTurnSpriteUnSelected;
    }

    //This function updates the current player and remaining bubbles 
    //displays in the top right corner of the screen.
    void UpdatePlayerAndBubbleDisplay()
    {
        bubblesRemainingText.text = bubblesRemaining.ToString();
        currentPlayerText.text = CurrentPlayerTurn.ToString();
    }

    //This method will deselect all squares on the board.
    void UnSelectAllSquares()
    {
        foreach(Square sq in squares)
        {
            sq.UnSelectThisSquare();
        }
    }

    void SelectSquaresEligibleToSeed()
    {
        foreach(Square sq in squares)
        {
            //Find the squares that are controlled by the current player
            if (sq.IsControlled && sq.PlayerControl == CurrentPlayerTurn)
            {
                //Then find the adjacent squares that are not controlled
                FindAdjacentUncontrolledSquaresAndSelectThem(sq);
            }
        }
    }

    //This method will select all squares that are uncontrolled 
    //and adjacent to the square passed as a parameter.
    void FindAdjacentUncontrolledSquaresAndSelectThem(Square sq)
    {
        //locate all adjacent squares
        for (int x = sq.Location.x-1; x <= sq.Location.x+1; x++)
        {
            for (int y = sq.Location.y - 1; y <= sq.Location.y+1; y++)
            {
                //Create a key to use to look in dictionary
                Vector2 key = new Vector2(x, y);

                //Look in Dictionary with key, if the reference to a square returned is NOT null, 
                // and not controlled, then select it.
                if (squareDictionary.TryGetValue(key, out value) != null)
                {
                    if(!value.IsControlled)
                    {
                        value.SelectThisSquare();
                    }
                }
            }
        }
    }


    #endregion
}
