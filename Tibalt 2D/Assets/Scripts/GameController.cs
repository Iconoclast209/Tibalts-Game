﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectedAction
{
    seed,
    stack,
    progress,
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
    [SerializeField]
    GameObject prefabUnitOfWar;
    [SerializeField]
    GameObject prefabUnitOfArt;
    [SerializeField]
    GameObject unitDestroyedMessage;
    [SerializeField]
    GameObject gameOverMessage;
    [SerializeField]
    Canvas canvas;


    //NOT visible in inspector
    int numberOfPlayers = 2;
    int currentPlayer = 1;
    int bubblesRemaining = 1;
    bool seedButtonClickedThisTurn = false;
    public Square[] allSquaresOnBoard;
    SelectedAction currentAction = SelectedAction.firstTurn;
    // 0 = selected color, all other ints relate to player number.
    Dictionary<int, Color> colorDictionary = new Dictionary<int, Color>();
    Color selectedColor = Color.yellow;
    Image seedButtonImage;
    Image stackButtonImage;
    Image progressButtonImage;
    Image endTurnButtonImage;
    Dictionary<Vector2Int, Square> squareDictionary = new Dictionary<Vector2Int, Square>();

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

    public Dictionary<Vector2Int, Square> SquareDictionary
    {
        get { return squareDictionary; }
    }

    #endregion

    #region METHODS

    void Start()
    {
        //Create an array of all the squares on the playing field.
        allSquaresOnBoard = FindObjectsOfType<Square>();
        Debug.Log("Found " + allSquaresOnBoard.Length + " squares.");
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    //This method will add all of the squares to the dictionary 
    //which are accessible by their positions relative to the bottom 
    //left corner of the board.
    void AddSquaresToDictionary()
    {
        squareDictionary.Clear();
        foreach (Square sq in allSquaresOnBoard)
        {
            try
            {
                squareDictionary.Add(sq.Location, sq);
            }
            catch (ArgumentException e)
            {
                Debug.Log(e.Message);
            }
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
            //Should only be able to click button once per turn.
            if(!seedButtonClickedThisTurn)
            {
                seedButtonClickedThisTurn = true;
                currentAction = SelectedAction.seed;
                UnSelectAllButtons();
                UnSelectAllSquares();
                seedButtonImage.sprite = seedSpriteSelected;
                SelectSquaresEligibleToSeed();
            }
            else
            {
                Debug.Log("You have already clicked the seed button once this turn.");
            }
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
            currentAction = SelectedAction.progress;
            
            UnSelectAllButtons();
            UnSelectAllSquares();
            progressButtonImage.sprite = progressSpriteSelected;
            Progress();
            //Invoke("ResetProgressButton", 1.0f);

        }
    }

    //This happens when you click the END TURN button.
    public void EndTurnButtonClicked()
    {
        currentAction = SelectedAction.hold;
        UnSelectAllButtons();
        UnSelectAllSquares();
        endTurnButtonImage.sprite = endTurnSpriteSelected;
        SetAllUnitsOfWarToNotRotate();

        if (!CheckForEndGame())
        {
            GoToNextPlayerTurn();
            Invoke("ResetEndTurnButton", 1.0f);
        }
        else
        {
            gameOverMessage.SetActive(true);
            gameOverMessage.GetComponent<Image>().color = ReturnPlayerColor(CurrentPlayer);
        }
        
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
        foreach (Square sq in allSquaresOnBoard)
        {
            sq.DeSelectThisSquare();
        }
    }
    
    //
    //This method will select all the squares that are eligible to be seeded.
    //
    void SelectSquaresEligibleToSeed()
    {
        foreach (Square sq in allSquaresOnBoard)
        {
            //Find the squares that are controlled by the current player
            if (sq.IsControlled && sq.PlayerControl == CurrentPlayer)
            {
                List<Square> squaresToSelect = FindAdjacentUncontrolledSquares(sq);
                if(squaresToSelect != null)
                {
                    SelectSquaresInList(squaresToSelect);
                }
            }
        }
    }

    //
    //This function will return a list of all the adjacent uncontrolled squares to 
    //whatever square is passed in as a parameter.
    //
    List<Square> FindAdjacentUncontrolledSquares(Square sq)
    {
        List<Square> adjacentUncontrolledSquares = new List<Square>();
        //locate all adjacent squares
        for (int x = sq.Location.x - 1; x <= sq.Location.x + 1; x++)
        {
            for (int y = sq.Location.y - 1; y <= sq.Location.y + 1; y++)
            {
                //Create a key to use to look in dictionary
                Vector2Int key = new Vector2Int(x, y);

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

    //
    //This method will select all the squares in a list of squares
    //
    void SelectSquaresInList(List<Square> listOfSquares)
    {
        foreach (Square sq in listOfSquares)
        {
            sq.SelectThisSquare();
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
            Debug.Log("Increment Player Number");
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
        endTurnButtonImage.sprite = endTurnSpriteUnSelected;
    }

    private void ResetProgressButton()
    {
        progressButtonImage.sprite = progressSpriteUnSelected;
    }

    //
    // This function will determine how many bubbles should be generated.
    //
    private void GenerateBubbles()
    {
        List<Square> squaresToGenerateBubbles = new List<Square>();

        foreach (Square sq in allSquaresOnBoard)
        {
            //Find the squares that are controlled by the current player
            if (sq.IsControlled && sq.PlayerControl == CurrentPlayer)
            {
                List<Square> adjacentSquares = FindAdjacentUncontrolledSquares(sq);
                foreach(Square squareInList in adjacentSquares)
                {
                    if (!squaresToGenerateBubbles.Contains(squareInList))
                    {
                        squaresToGenerateBubbles.Add(squareInList);
                        Debug.Log("Square added to squaresToGenerateBubble");
                    }
                    else
                    {
                        Debug.Log("Square is already in the list squaresToGenerateBubbles.");
                    }
                }
            }
        }
        //Set bubblesRemaining to the number of squares added to the list.
        bubblesRemaining = squaresToGenerateBubbles.Count / 2;
    }


    private void StartNewTurn()
    {
        Debug.Log("Starting a new turn!");
        seedButtonClickedThisTurn = false;
        bubblesRemaining = 0;
        GenerateBubbles();
        if(CurrentAction == SelectedAction.firstTurn)
        {
            bubblesRemaining = 1;
        }
        UpdatePlayerAndBubbleDisplay();
        MoveAllUnitsOfWarForCurrentPlayer();
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
        foreach (Square sq in allSquaresOnBoard)
        {
            //Find the squares that are controlled by the current player
            if (sq.IsControlled && sq.PlayerControl == CurrentPlayer)
            {
                Debug.Log("Found a controlled square!");
                if(sq.BubblesStacked % 2 == 0)
                {
                    //This is an even number, create a Unit of War
                    CreateUnit(sq, prefabUnitOfWar);
                }
                else if(sq.BubblesStacked % 3 ==0)
                {
                    //This is an odd number divisible by 3, create a Unit of Art
                    CreateUnit(sq, prefabUnitOfArt);
                }
                else
                {
                    //The square will remain as-is.
                }
            }
        }
    }

    void CreateUnit(Square sq, GameObject prefab)
    {
        Debug.Log("Creating Unit at " + sq.Location.ToString());
        Vector2Int spawnPosition = DetermineSpawnPosition(sq);
        GameObject newUnit = Instantiate(prefab, canvas.GetComponent<RectTransform>());
        newUnit.GetComponent<RectTransform>().anchoredPosition = spawnPosition;
        newUnit.GetComponent<Unit>().SetupUnit(sq);
    }

    //
    //Determine Spawn Position for Units of Art or War
    //
    Vector2Int DetermineSpawnPosition(Square sq)
    {
        RectTransform rt = sq.GetComponent<RectTransform>();
        return new Vector2Int((int)rt.anchoredPosition.x, (int)rt.anchoredPosition.y);
    }


    void MoveAllUnitsOfWarForCurrentPlayer()
    {
        War[] unitsOfWarOnBoard = FindObjectsOfType<War>();
        foreach (War unitOfWar in unitsOfWarOnBoard)
        {
            if (unitOfWar.PlayerControl == CurrentPlayer)
            {
                unitOfWar.AttemptToMove();
            }
        }
    }

    public void ShowUnitDestroyedMessage()
    {
        unitDestroyedMessage.SetActive(true);
        Invoke("HideUnitDestroyedMessage", 1.0f);

    }

    void HideUnitDestroyedMessage()
    {
        unitDestroyedMessage.SetActive(false);
    }

    void SetAllUnitsOfWarToNotRotate()
    {
        War[] unitsOfWar = FindObjectsOfType<War>();
        foreach (War unit in unitsOfWar)
        {
            if(unit.PlayerControl == CurrentPlayer)
            {
                unit.SetCanBeRotatedToFalse();
            }
        }

    }

    //Is the game over? true = yes
    bool CheckForEndGame()
    {
        Debug.Log("Check for End of Game.");
        int opposingPlayerControlledSquares = 0;
        foreach (Square sq in allSquaresOnBoard)
        {
            if(sq.IsControlled && sq.PlayerControl != CurrentPlayer)
            {
                opposingPlayerControlledSquares++;
            }
        }

        Debug.Log("Found this many opposing controlled squares: " + opposingPlayerControlledSquares.ToString());

        if (opposingPlayerControlledSquares <= 0)
        {
            Debug.Log("Game has ended.");
            return true;
        }
        else
        {
            Debug.Log("Game will continue.");
            return false;
        }
    }

    #endregion
}
