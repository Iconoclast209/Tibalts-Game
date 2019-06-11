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
    Dictionary<Vector2, GameObject> squareDictioanry = new Dictionary<Vector2, GameObject>();
    


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
        squares = FindObjectsOfType<Square>();
        Debug.Log("Found " + squares.Length + " squares.");
        seedButtonImage = seedButton.GetComponent<Image>();
        stackButtonImage = stackButton.GetComponent<Image>();
        progressButtonImage = progressButton.GetComponent<Image>();
        endTurnButtonImage = endTurnButton.GetComponent<Image>();
        UpdatePlayerAndBubbleDisplay();
        AddSquaresToDictionary();
    }

    void AddSquaresToDictionary()
    {
        foreach (Square sq in squares)
        {
            squareDictioanry.Add(sq.Location, sq.gameObject);
        }
    }

    public Color ReturnCurrentPlayerColor()
    {
        if (CurrentPlayerTurn == 1)
            return player1Color;
        else
            return player2Color;
    }

    public void SeedButtonClicked()
    {
        currentAction = SelectedAction.seed;
        UnSelectAllButtons();
        UnSelectAllSquares();
        seedButtonImage.sprite = seedSpriteSelected;
    }

    public void StackButtonClicked()
    {
        currentAction = SelectedAction.stack;
        UnSelectAllButtons();
        UnSelectAllSquares();
        stackButtonImage.sprite = stackSpriteSelected;
    }

    public void ProgressButtonClicked()
    {
        currentAction = SelectedAction.hold;
        UnSelectAllButtons();
        UnSelectAllSquares();
        progressButtonImage.sprite = progressSpriteSelected;
    }

    public void EndTurnButtonClicked()
    {
        currentAction = SelectedAction.hold;
        UnSelectAllButtons();
        UnSelectAllSquares();
        endTurnButtonImage.sprite = endTurnSpriteSelected;
    }

    void UnSelectAllButtons()
    {
        seedButtonImage.sprite = seedSpriteUnSelected;
        stackButtonImage.sprite = stackSpriteUnSelected;
        progressButtonImage.sprite = progressSpriteUnSelected;
        endTurnButtonImage.sprite = endTurnSpriteUnSelected;
    }

    void UpdatePlayerAndBubbleDisplay()
    {
        bubblesRemainingText.text = bubblesRemaining.ToString();
        currentPlayerText.text = CurrentPlayerTurn.ToString();
    }

    void UnSelectAllSquares()
    {
        foreach(Square sq in squares)
        {
            sq.UnSelectThisSquare();
        }
    }

    void SelectSquaresEligibleToSeed()
    {
        // (3,3)
        foreach(Square sq in squares)
        {
            //Find the squares that are controlled by the current player
            if (sq.IsControlled && sq.PlayerControl == CurrentPlayerTurn)
            {
                //Then find the adjacent squares that are not controlled
            }
        }
    }

    #endregion
}
