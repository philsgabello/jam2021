using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{

    public Transform[] cardSockets;
    Dictionary<Transform, Card> slotsDictionary = new Dictionary<Transform, Card>();

    List<int> slotsToRemove = new List<int>();


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
        
    }

    public Transform GetSocket(int i)
    {
        return cardSockets[i];
    }

    public bool IsSlotAvailable(int i)
    {
        return slotsDictionary[GetSocket(i)] == null;
    }

    public bool BookSlot(int i, Card newCard, bool forceRelease = false)
    {
        if(!IsSlotAvailable(i))
        {

            return false;
            //if (forceRelease)
            //{
            //    AppendSlotRemoval(i);
            //}
            //else
            //{
            //    return false;
            //}
        }

        slotsDictionary[GetSocket(i)] = newCard;
        //newCard.AssignSlot(cardSockets[i]);

        return true;
    }

    public void AppendSlotRemoval(int i)
    {
        slotsToRemove.Add(i);
    }

    public void ReleaseSlots()
    {
        foreach(int i in slotsToRemove)
        {
            if (slotsDictionary[GetSocket(i)] != null)
            {
                slotsDictionary[GetSocket(i)].ClearSlot();
                slotsDictionary[GetSocket(i)] = null;
            }
        }

        
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
