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
    Image image;
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
       
    }

   
    public void HandleClick()
    {
        Debug.Log("Mouse Clicked.");
        if(!isSelected)
        {
            SelectThisSquare();
        }
        else
        {
            if (!isControlled)
            {
                TakeOwnership(3, Color.green);
            }
        }
        
    }

    void SelectThisSquare()
    {
        isSelected = true;
        image.color = gameController.SelectedColor;
        Debug.Log("This square has been selected.");
    }
    
    //This function will take ownership of a square and stack one bubble on it.
    void TakeOwnership(int player, Color color)
    {
        isControlled = true;
        playerControl = player;
        bubblesStacked++;
        image.color = color;
    }
}
