using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected Sprite sprite;
    protected int strength = 0;
    protected Square hostSquare; //assume this will be needed, will find out later

    public int Strength
    {
        get { return strength; }
    }

    public Square HostSquare
    {
        get { return hostSquare; }
    }

    public void SetupUnit(Square sq)
    {
        hostSquare = sq;
        strength = sq.BubblesStacked;
        sq.ConvertToUnit();
    }


}
