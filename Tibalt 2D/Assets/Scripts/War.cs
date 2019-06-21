using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class War : Unit
{
    Vector2 directionOfMovement;


    //
    // CONSTRUCTOR
    // Need to consider the order of operations for PROGRESS, 
    // and the mechanism for setting the initial direction of the unit of war.
    // Initial thought - click multiple times on each unit of war to orient the unit 
    // and then hit a button to complete/end the process.
    //
    public War(Square sq)
    {
        base.hostSquare = sq;
        base.strength = sq.BubblesStacked;
    }

    void SelectDirection()
    {

    }

    void Move()
    {

    }
}
