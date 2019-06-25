using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class War : Unit
{
    [SerializeField]
    Vector2Int[] vectorDirectionArray;
    
    RectTransform rectTransform;
    int currentVectorIndex = 0;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        text = GetComponentInChildren<Text>();
    }

    public void HandleClick()
    {
        //Handle a click on the Unit of War immediately after it is created
        //TODO Determine how to prevent rotation until it hits the edge of the board or a Unit of Art above its kill strength.
        if(gameController.CurrentAction == SelectedAction.progress)
        {
            Debug.Log("Processing Mouse Click on Unit of War.");
            RotateUnit();
        }
    }

    void RotateUnit()
    {
        do
        {
            IncrementVectorIndex();
            if (currentVectorIndex == 0)
            {
                rectTransform.rotation = Quaternion.identity;
                text.GetComponent<rectTransform>().rotation = Quaternion.identity;
            }
            else
            {
                // Rotate Image by 45 degrees each time it is clicked on.
                rectTransform.Rotate(0f, 0f, -45f);
                // Also, counter rotate text to make it readable.
                text.GetComponent<rectTransform>().Rotate(0f, 0f, 45f);
            }
        } while (!CheckForValidDirection())  //Check to see if direction is a valid direction to move, otherwise rotate one more time.
    }

    void IncrementVectorIndex()
    {
        currentVectorIndex++;

        if (currentVectorIndex > vectorDirectionArray.Length)
        {
            currentVectorIndex = 0;
        }
    }

    public bool CheckForValidDirection()
    {
        // Add the current direction vector to the hostSquare's location vector.
        Vector2Int destinationSquareLocation = hostSquare.Location + vectorDirectionArray[currentVectorIndex];
        // Look in dictionary to see if the destination location exists on the board
        gameController.squareDictionary.TryGetValue(key, out Square currentSquare);
        // TODO may need to check if there is a unit of art with greater strength?
        if (currentSquare != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Move()
    {

    }
}
