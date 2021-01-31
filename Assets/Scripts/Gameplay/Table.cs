using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{

    public Transform[] cardSockets;
    Dictionary<Transform, Card> slotsDictionary = new Dictionary<Transform, Card>();


    List<int> cardsToRemove = new List<int>();


    void Start()
    {
        foreach (Transform t in cardSockets)
        {
            slotsDictionary.Add(t, null);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        string debug = "";
        int i = 0;
        foreach(Card c in slotsDictionary.Values)
        {
            debug = debug + "(" + i + ") " +  (c == null) + " - ";
            i++;
        }
        //Debug.Log(debug);
    }

    public Transform GetSocket(int i)
    {
        return cardSockets[i];
    }

    public bool IsSlotAvailable(int i)
    {

        return ((slotsDictionary[GetSocket(i)] == null) && (!cardsToRemove.Contains(i)));
    }

    public bool BookSlot(int i, Card newCard, bool forceRelease = false)
    {
        if(!IsSlotAvailable(i))
        {

            return false;
        }

        slotsDictionary[GetSocket(i)] = newCard;

        return true;
    }

    public void AppendSlotRemoval(int i)
    {
        cardsToRemove.Add(i);
        
    }

    public void ReleaseSlots()
    {
        foreach (int i in cardsToRemove)
        {
            
            slotsDictionary[GetSocket(i)].ClearSlot();
            slotsDictionary[GetSocket(i)] = null;
        }
        cardsToRemove.Clear();

        
    }

    public void ApplyCardsToSlot()
    {
        ReleaseSlots();
        foreach(KeyValuePair<Transform, Card> pair in slotsDictionary)
        {
            if(pair.Value != null)
            {
                pair.Value.AssignSlot(pair.Key);
            }
            
        }
    }



}
