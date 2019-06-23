using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{

    protected int strength = 0;
    protected Square hostSquare; //assume this will be needed, will find out later
    public Image image;
    public Text text;


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
        image.color = sq.ReturnCurrentColor();
        UpdateTextValue();
        sq.ConvertToUnit();

    }

    public void UpdateTextValue()
    {
        text.text = strength.ToString();
    }

}
