using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class War : Unit
{
    Vector2 directionOfMovement;
    GameController gameController;


    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }
    public void HandleClick()
    {
        //Handle a click on the Unit of War
        if(gameController.CurrentAction == SelectedAction.progress)
        {
            Debug.Log("Processing Mouse Click on Unit of War.");
        }
    }

    void SelectDirection()
    {

    }

    void Move()
    {

    }
}
