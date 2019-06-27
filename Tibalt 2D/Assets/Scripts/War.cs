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
    bool canBeRotated = true;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        text = GetComponentInChildren<Text>();
    }

    public void HandleClick()
    {
        //Handle a click on the Unit of War immediately after it is created
        Debug.Log("Processing Mouse Click on Unit of War.");
        RotateUnit();
    }

    void RotateUnit()
    {
        if(canBeRotated)
        {
            do
            {
                IncrementVectorIndex();
                if (currentVectorIndex == 0)
                {
                    rectTransform.rotation = Quaternion.identity;
                    text.GetComponent<RectTransform>().rotation = Quaternion.identity;
                }
                else
                {
                    // Rotate Image by 45 degrees each time it is clicked on.
                    rectTransform.Rotate(0f, 0f, -45f);
                    // Also, counter rotate text to make it readable.
                    text.GetComponent<RectTransform>().Rotate(0f, 0f, 45f);
                }
            } while (!CheckForValidDirection());  //Check to see if direction is a valid direction to move, otherwise rotate one more time.
        }
    }

    void IncrementVectorIndex()
    {
        currentVectorIndex++;

        if (currentVectorIndex > vectorDirectionArray.Length-1)
        {
            currentVectorIndex = 0;
        }
    }

    public bool CheckForValidDirection()
    {
        Debug.Log("Current Vector Index is " + currentVectorIndex.ToString());
        // Add the current direction vector to the hostSquare's location vector.
        Vector2Int destinationSquareLocation = hostSquare.Location + vectorDirectionArray[currentVectorIndex];
        // Look in dictionary to see if the destination location exists on the board
        gameController.SquareDictionary.TryGetValue(destinationSquareLocation, out Square targetSquare);
        if (targetSquare != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AttemptToMove()
    {
        Debug.Log("Unit of War Attempt to Move.");
        if(CheckForValidDirection())
        {
            // Identify the target square to move unit of war to
            Vector2Int destinationSquareLocation = hostSquare.Location + vectorDirectionArray[currentVectorIndex];
            gameController.SquareDictionary.TryGetValue(destinationSquareLocation, out Square targetSquare);

            //Find all units, then Check to see if a unit is present in the target square
            Unit[] unitsOnBoard = FindObjectsOfType<Unit>();
            Unit targetUnit = null;
            foreach (Unit unit in unitsOnBoard)
            {
                if(unit.HostSquare == targetSquare)
                {
                    targetUnit = unit;
                    break;
                }
            }

            if (targetUnit != null)
            {
                if(targetUnit.GetComponent<Art>() != null)
                {
                    if(Strength >= (2 * targetUnit.Strength))
                    {
                        Destroy(targetUnit.gameObject);
                        Move(targetSquare);
                    }
                    else
                    {
                        Destroy(this.gameObject);
                        gameController.ShowUnitDestroyedMessage();
                    }
                }
                else
                {
                    // Compare strength
                    if (Strength > targetUnit.Strength)
                    {
                        //This unit beats the target Unit and move proceeds.
                        Destroy(targetUnit.gameObject);
                        Move(targetSquare);
                    }
                    else if (Strength == targetUnit.Strength)
                    {
                        //Stalemate, this unit does not move.
                        //Flash a message
                        return;
                    }
                    else
                    {
                        Destroy(this.gameObject);
                        gameController.ShowUnitDestroyedMessage();
                    }
                }
            }
            else
            {
                Move(targetSquare);
            }
        }
        else
        {
            // Message and allow the unit to be redirected.
            Debug.Log("canBeRotated is set to true");
            canBeRotated = true;
        }
    }

    private void Move(Square targetSquare)
    {
        // Move the unit of war
        rectTransform.anchoredPosition = targetSquare.GetComponent<RectTransform>().anchoredPosition;
        // Update the host square for the unit
        hostSquare = targetSquare;
        hostSquare.DestroyThisSquare();
    }

    public void SetCanBeRotatedToFalse()
    {
        canBeRotated = false;
    }
}
