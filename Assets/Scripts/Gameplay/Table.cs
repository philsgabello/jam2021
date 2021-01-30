using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{

    public Transform[] cardSockets;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetSocket(int i)
    {
        return cardSockets[i];
    }

}
