using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class War : Unit
{
    [SerializeField]
    Vector2Int[] vectorArray;
    
    GameController gameController;
    RectTransform rectTransform;
    int currentVectorIndex = 0;


    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        rectTransform = GetComponent<RectTransform>();
        text = GetComponentInChildren<Text>();
    }

    public void HandleClick()
    {
        //Handle a click on the Unit of War
        if(gameController.CurrentAction == SelectedAction.progress)
        {
            Debug.Log("Processing Mouse Click on Unit of War.");
            //Rotate and set direction.
            currentVectorIndex++;

            if (currentVectorIndex > vectorArray.Length)
            {
                currentVectorIndex = 0;
            }

            RotateUnit();
        }
    }

    void RotateUnit()
    {
        if(currentVectorIndex == 0)
        {
            rectTransform.rotation = Quaternion.identity;
        }
        else
        {
            rectTransform.Rotate(0f, 0f, -45f);
        }
        //Check to see if direction is a valid direction to move, otherwise rotate one more time.
    }

    void Move()
    {

    }
}
