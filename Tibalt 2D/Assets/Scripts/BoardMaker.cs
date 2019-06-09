using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardMaker : MonoBehaviour
{
    [SerializeField]
    GameObject squarePrefab;

    
    // Start is called before the first frame update
    void Start()
    {
        SetupBoard();
    }

    void SetupBoard()
    {
        for(int x = 0; x<9; x++)
        {
            for(int y=0;y<9;y++)
            {
                GameObject newSquare = Instantiate(squarePrefab);
                RectTransform newSquareRT = newSquare.GetComponent<RectTransform>();
                newSquareRT.SetParent(this.GetComponent<RectTransform>());
                newSquareRT.anchoredPosition = new Vector2((x * 100) - 400, (y * 100) - 400);
            }
        }
            
    }
}
