using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectedAction
{
    seed,
    stack,
    hold,
    firstTurn,
    buttonsDisabled
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
    [SerializeField]
    GameObject firstTurn;

    //NOT visible in inspector
    int numberOfPlayers = 2;
    int currentPlayer = 1;
    int bubblesRemaining = 0;
    public Square[] squares;
    SelectedAction currentAction = SelectedAction.firstTurn;
    // 0 = selected color, all other ints relate to player number.
    Dictionary<int, Color> colorDictionary = new Dictionary<int, Color>();
    Color selectedColor = Color.yellow;
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

    public int CurrentPlayer
    {
        get { return currentPlayer; }
    }

    public int NumberOfPlayers
    {
        get { return numberOfPlayers; }
    }

    public Color SelectedColor
    {
        get { return selectedColor; }
    }

    public int BubblesRemaining
    {
        get { return bubblesRemaining; }
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
        //Build a dictionary of all the squares 
        AddSquaresToDictionary();
        //Generate the color entries in the colorDictionary
        SetupColorDictionary();

        currentPlayer = 1;
        //Update the current player and bubble display
        UpdatePlayerAndBubbleDisplay();
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
    public Color ReturnPlayerColor(int playerNum)
    {
        colorDictionary.TryGetValue(playerNum, out Color value);
        return value;
    }

    //This happens when you click the SEED button.
    public void SeedButtonClicked()
    {
        if (currentAction != SelectedAction.firstTurn)
        {
            currentAction = SelectedAction.seed;
            UnSelectAllButtons();
            UnSelectAllSquares();
            seedButtonImage.sprite = seedSpriteSelected;
            SelectSquaresEligibleToSeed();
        }
    }

    //This happens when you click the STACK button.
    public void StackButtonClicked()
    {
        if (currentAction != SelectedAction.firstTurn)
        {
            currentAction = SelectedAction.stack;
            UnSelectAllButtons();
            UnSelectAllSquares();
            stackButtonImage.sprite = stackSpriteSelected;
        }
    }

    //This happens when you click the PROGRESS button.
    public void ProgressButtonClicked()
    {
        if (currentAction != SelectedAction.firstTurn)
        {
            currentAction = SelectedAction.hold;
            
            UnSelectAllButtons();
            UnSelectAllSquares();
            progressButtonImage.sprite = progressSpriteSelected;
            Progress();
            Invoke("ResetProgressButton", 1.0f);

        }
    }

    //This happens when you click the END TURN button.
    public void EndTurnButtonClicked()
    {
        currentAction = SelectedAction.hold;
        UnSelectAllButtons();
        UnSelectAllSquares();
        endTurnButtonImage.sprite = endTurnSpriteSelected;
        GoToNextPlayerTurn();
        Invoke("ResetEndTurnButton", 1.0f);
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
        currentPlayerText.text = CurrentPlayer.ToString();
        currentPlayerText.gameObject.GetComponentInParent<Image>().color = ReturnPlayerColor(CurrentPlayer);
    }

    //This method will deselect all squares on the board.
    public void UnSelectAllSquares()
    {
        foreach (Square sq in squares)
        {
            sq.DeSelectThisSquare();
        }
    }

    void SelectSquaresEligibleToSeed()
    {
        foreach (Square sq in squares)
        {
            //Find the squares that are controlled by the current player
            if (sq.IsControlled && sq.PlayerControl == CurrentPlayer)
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
        for (int x = (int)sq.Location.x - 1; x <= (int)sq.Location.x + 1; x++)
        {
            for (int y = (int)sq.Location.y - 1; y <= (int)sq.Location.y + 1; y++)
            {
                //Create a key to use to look in dictionary
                Vector2 key = new Vector2(x, y);

                //Look in Dictionary with key, if the reference to a square returned is NOT null, 
                // and not controlled, then select it.
                squareDictionary.TryGetValue(key, out Square currentSquare);
                if (currentSquare != null && !currentSquare.IsControlled)
                {
                    currentSquare.SelectThisSquare();
                }
            }
        }
    }

    //This function will return a list of all the adjacent uncontrolled squares to 
    //whatever square is passed in as a parameter.
    List<Square> FindAdjacentUncontrolledSquares(Square sq)
    {
        List<Square> adjacentUncontrolledSquares = new List<Square>();
        //locate all adjacent squares
        for (int x = (int)sq.Location.x - 1; x <= (int)sq.Location.x + 1; x++)
        {
            for (int y = (int)sq.Location.y - 1; y <= (int)sq.Location.y + 1; y++)
            {
                //Create a key to use to look in dictionary
                Vector2 key = new Vector2(x, y);

                //Look in Dictionary with key, if the reference to a square returned is NOT null, 
                // and not controlled, then add it to the List.
                squareDictionary.TryGetValue(key, out Square currentSquare);
                if (currentSquare != null && !currentSquare.IsControlled  && !currentSquare.IsDepleted)
                {
                    adjacentUncontrolledSquares.Add(currentSquare);
                }
            }
        }
        //If there are adjacent uncontrolled squares, return the List
        if(adjacentUncontrolledSquares.Count > 0)
        {
            return adjacentUncontrolledSquares;
        }
        else
        {
            return null;
        }
    }


    public void GoToNextPlayerTurn()
    {
        if (CurrentPlayer == NumberOfPlayers || CurrentPlayer <= 0)
        {
            currentPlayer = 1;
        }
        else
        {
            currentPlayer++;
        }
        StartNewTurn();
    }

    private void SetupColorDictionary()
    {
        colorDictionary.Add(1, Color.green);
        colorDictionary.Add(2, Color.red);
    }

    public void EndFirstTurn()
    {
        firstTurn.SetActive(false);
        currentAction = SelectedAction.hold;
        currentPlayer = 1;
        GenerateBubbles();
        UpdatePlayerAndBubbleDisplay();
    }

    private void ResetEndTurnButton()
    {
        //Deselect the End Turn Button
        endTurnButtonImage.sprite = endTurnSpriteUnSelected;
    }

    private void ResetProgressButton()
    {
        progressButtonImage.sprite = progressSpriteUnSelected;
    }

    private void GenerateBubbles()
    {
        List<Square> squaresToGenerateBubbles = new List<Square>();


        foreach (Square sq in squares)
        {
            //Find the squares that are controlled by the current player
            if (sq.IsControlled && sq.PlayerControl == CurrentPlayer)
            {
                //
                //REPLACE WITH METHOD CALL TO FindAdjacentUncontrolledSquares()
                //
                /*
                Debug.Log("Found a controlled square!!!!!!!!!!!");
                //Then find the adjacent squares that are not controlled.
                for (int x = (int)sq.Location.x - 1; x <= (int)sq.Location.x + 1; x++)
                {
                    for (int y = (int)sq.Location.y - 1; y <= (int)sq.Location.y + 1; y++)
                    {
                        //Create a key to use to look in dictionary
                        Vector2 key = new Vector2(x, y);
                        Debug.Log("Testing a Square to generate bubbles.");
                        
                        squareDictionary.TryGetValue(key, out Square currentSquare);

                        if (currentSquare != null)
                        {
                            Debug.Log("Found a square in dictionary.");
                            if (!currentSquare.IsControlled)
                            {
                                if(!squaresToGenerateBubbles.Contains(currentSquare))
                                {
                                    squaresToGenerateBubbles.Add(currentSquare);
                                    Debug.Log("currentSquare added to squaresToGenerateBubble at " + key.ToString());
                                }
                                else
                                {
                                    Debug.Log("currentSquare is already in the list squaresToGenerateBubbles.");
                                }
                            }
                            else
                            {
                                Debug.Log("Square at " + key.ToString() + " is controlled by a player");
                            }
                        }
                    }
                }*/
            }
        }
        //Set bubblesRemaining to the number of squares added to the list.
        bubblesRemaining = squaresToGenerateBubbles.Count / 2;
    }


    private void StartNewTurn()
    {
        Debug.Log("Starting a new turn!");
        bubblesRemaining = 0;
        GenerateBubbles();
        UpdatePlayerAndBubbleDisplay();
    }

    public void UseABubble()
    {
        if (bubblesRemaining > 0)
        {
            bubblesRemaining--;
            UpdatePlayerAndBubbleDisplay();
        }
    }

    private void Progress()
    {
        foreach (Square sq in squares)
        {
            //Find the squares that are controlled by the current player
            if (sq.IsControlled && sq.PlayerControl == CurrentPlayer)
            {
                Debug.Log("Found a controlled square!");
                if(sq.BubblesStacked % 2 == 0)
                {
                    //This is an even number, create a Unit of War
                    Debug.Log("Creating Unit of War at " + sq.Location.ToString());
                }
                else if(sq.BubblesStacked % 3 ==0)
                {
                    //This is an odd number divisible by 3, create a Unit of Art
                    Debug.Log("Creating Unit of Art at " + sq.Location.ToString());
                }
                else
                {
                    //The square will remain as-is.
                }
                
                
            }
        }
    }

    #endregion
}
