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
    GameObject seedButton;



    //NOT visible in inspector
    int numberOfPlayers = 2;
    int playerTurn = 0;
    public Square[] squares;
    SelectedAction currentAction = SelectedAction.hold;
    Color selectedColor = Color.yellow;
    


    #endregion

    #region PROPERTIES
    
    public SelectedAction CurrentAction
    {
        get { return currentAction; }
    }

    public int PlayerTurn
    {
        get { return playerTurn; }
    }

    public Color SelectedColor
    {
        get { return selectedColor; }
    }



    #endregion

    #region METHODS
    // Start is called before the first frame update
    void Start()
    {
        squares = FindObjectsOfType<Square>();
        Debug.Log("Found " + squares.Length + " squares.");
        Debug.Log(SelectedAction.seed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SeedButtonClicked()
    {
        currentAction = SelectedAction.seed;
    

    }
    #endregion
}
