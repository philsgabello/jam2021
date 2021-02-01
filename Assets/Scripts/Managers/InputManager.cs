using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputManager : MonoBehaviour
{
    public string CurrentInput
    {
        get { return currentString; }
    }

    public static InputManager instance;

    string currentString = ""; 

    KeyCode[] keyCodes = {  KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L,
                            KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, 
                            KeyCode.Y, KeyCode.Z};


    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        

        foreach(KeyCode key in keyCodes)
        {
            if (Input.GetKeyDown(key))
            {
                currentString = currentString + key.ToString();
            }
            if (Input.GetKeyUp(key))
            {
                currentString = currentString.Replace(key.ToString(), "");
            }
        }

        Debug.Log(currentString);



    }


    
}
