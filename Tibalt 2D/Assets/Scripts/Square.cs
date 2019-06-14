using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour
{
    #region FIELDS
    bool isDepleted = false;
    bool isControlled = false;
    bool isSelected = false;
    int playerControl = 0;
    int bubblesStacked = 0;
    Vector2 location;
    Image image;
    Text text;
    RectTransform rectTransform;
    GameController gameController;

    #endregion

    #region PROPERTIES
    public bool IsDepleted
    {
        get { return isDepleted; }
    }

    public bool IsControlled
    {
        get { return isControlled; }
    }

    public Vector2 Location
    {
        get { return location; }
    }

    public int PlayerControl
    {
        get { return playerControl; }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Set References and populate location field.
        image = GetComponent<Image>();
        gameController = FindObjectOfType<GameController>();
        rectTransform = GetComponent<RectTransform>();
        text = gameObject.GetComponentInChildren<Text>();
        text.gameObject.SetActive(false);
        location = new Vector2((rectTransform.anchoredPosition.x + 400) / 100, (rectTransform.anchoredPosition.y + 400) / 100);
    }

   
    public void HandleClick()
    {
        //This handles actions on the first turn.  Each player can select one square to seed.
        if (gameController.CurrentAction == SelectedAction.firstTurn)
        {
            FirstTurn();
            return;
        }

        if (!isSelected && gameController.CurrentAction != SelectedAction.hold)
        {
            //if(gameController.CurrentAction == SelectedAction.seed)
            SelectThisSquare();
        }
        else
        {
            if(gameController.CurrentAction == SelectedAction.seed && isControlled == false)
            {
                SeedThisSquare();
            }
            else if(gameController.CurrentAction == SelectedAction.stack && playerControl == gameController.CurrentPlayer)
            {
                StackThisSquare();
            }
        }
        
    }

    void FirstTurn()
    {
        if (!isSelected)
        {
            SelectThisSquare();
        }
        else
        {
            SeedThisSquare();
            if (gameController.CurrentPlayer >= gameController.NumberOfPlayers)
            {
                //First Turn image deactivated.
                gameController.EndFirstTurn();
            }
            else
            {
                //Change to next player.
                gameController.GoToNextPlayerTurn();
            }
            return;
        }
    }

    public void SelectThisSquare()
    {
        //If the Current Action is not SEED, then only one square should be selected at a time.
        if (gameController.CurrentAction != SelectedAction.seed)
        {
            gameController.UnSelectAllSquares();
        }
        isSelected = true;
        //
        //TO DO:  Replace with a outline indicator rather than color, since
        //        selecting a cell can occur with a controlled cell, or a cell with a unit on it.
        image.color = gameController.SelectedColor;
    }

    public void DeSelectThisSquare()
    {
        isSelected = false;
        //
        //TO DO:  Revert to previous/base sprite.
        //        
        if(!isControlled)
        {
            image.color = Color.white;
        }
        else
        {
            image.color = gameController.ReturnPlayerColor(playerControl);
        }

    }

    void StackThisSquare()
    {
        if(text.IsActive() == false)
        {
            text.gameObject.SetActive(true);
        }
        bubblesStacked++;
        text.text = bubblesStacked.ToString();
    }

    //This function will take ownership of a square and stack one bubble on it.
    void SeedThisSquare()
    {
        isControlled = true;
        //Set controlled by current player
        playerControl = gameController.CurrentPlayer;
        //Add one bubble to the square's stack
        bubblesStacked++;
        if (text.IsActive() == false)
        {
            text.gameObject.SetActive(true);
        }
        text.text = bubblesStacked.ToString();

        // Set the square's color to the player color.
        image.color = gameController.ReturnPlayerColor(playerControl);
    }
}
