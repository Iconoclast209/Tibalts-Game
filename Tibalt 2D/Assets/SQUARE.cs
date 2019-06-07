using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQUARE : MonoBehaviour
{
    bool isDepleted = false;
    bool isControlled = false;
    int playerControl = 0;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Square Clicked!");
    }
}
