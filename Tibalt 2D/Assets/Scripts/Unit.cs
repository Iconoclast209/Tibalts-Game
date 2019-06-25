using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{

    protected int strength = 0;
    protected Square hostSquare;
    protected int playerControl;
    protected GameController gameController;
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

    public int PlayerControl
    {
        get { return playerControl; }
    }

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public void SetupUnit(Square sq)
    {
        hostSquare = sq;
        strength = sq.BubblesStacked;
        image.color = sq.ReturnCurrentColor();
        playerControl = gameController.CurrentPlayer;
        UpdateTextValue();
        sq.ConvertToUnit();
    }

    public void UpdateTextValue()
    {
        text.text = strength.ToString();
    }

}
