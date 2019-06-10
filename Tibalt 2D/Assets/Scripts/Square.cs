﻿using System.Collections;
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

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        gameController = FindObjectOfType<GameController>();
        rectTransform = GetComponent<RectTransform>();
        text = gameObject.GetComponentInChildren<Text>();
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
        image.color = gameController.SelectedColor;
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
        // Set the square's color to the player color.
        image.color = gameController.ReturnCurrentPlayerColor();

    }
}
