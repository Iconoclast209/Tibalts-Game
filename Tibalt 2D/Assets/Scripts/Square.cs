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
        image = GetComponent<Image>();
        gameController = FindObjectOfType<GameController>();
        rectTransform = GetComponent<RectTransform>();
        text = gameObject.GetComponentInChildren<Text>();
        //TO DO:  Verify the line below.
        text.SetActive(false);
        location = new Vector2((rectTransform.anchoredPosition.x + 400) / 100, (rectTransform.anchoredPosition.y + 400) / 100);
    }

   
    public void HandleClick()
    {
        if(!isSelected)
        {
            SelectThisSquare();
        }
        else
        {
            if(gameController.CurrentAction == SelectedAction.seed && isControlled == false)
            {
                SeedThisSquare();
            }
            else if(gameController.CurrentAction == SelectedAction.stack && playerControl == gameController.CurrentPlayerTurn)
            {
                StackThisSquare();
            }
        }
        
    }

    void SelectThisSquare()
    {
        isSelected = true;
        //
        //TO DO:  Replace with a outline indicator rather than color, since
        //        selecting a cell can occur with a controlled cell, or a cell with a unit on it.
        image.color = gameController.SelectedColor;
    }

    public void UnSelectThisSquare()
    {
        isSelected = false;
        //
        //TO DO:  Revert to previous sprite.
        //        
        image.color = Color.white;
    }

    void StackThisSquare()
    {
        bubblesStacked++;
        text.text = bubblesStacked.ToString();
    }

    //This function will take ownership of a square and stack one bubble on it.
    void SeedThisSquare()
    {
        isControlled = true;
        //Set controlled by current player
        playerControl = gameController.CurrentPlayerTurn;
        //Add one bubble to the squre's stack
        bubblesStacked++;
        text.text = bubblesStacked.ToString();
        //TO DO: Replace with sprite for player color.
        // Set the square's color to the player color.
        image.color = gameController.ReturnCurrentPlayerColor();

    }
}
