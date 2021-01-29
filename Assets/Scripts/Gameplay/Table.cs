using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{



    struct SocketData
    {
        public Transform socketTransform;
        public Card associatedCard;
        public bool isAvailable;

    }

    public Transform[] cardSockets;
    List<SocketData> socketData = new List<SocketData>();

    void Start()
    {
        for(int i = 0; i < cardSockets.Length; i++)
        {
            SocketData newSocketData = new SocketData()
            {
                socketTransform = cardSockets[i],
                associatedCard = null,
                isAvailable = true
            };

            socketData.Add(newSocketData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignCardToSocket(Card card, int socketSlot)
    {
        SocketData updatedSocketData = new SocketData()
        {
            socketTransform = socketData[socketSlot].socketTransform,
            associatedCard = card,
            isAvailable = false
        };

        socketData[socketSlot] = updatedSocketData;

    }

    public void ResetSocket(int socketSlot)
    {
        SocketData updatedSocketData = new SocketData()
        {
            socketTransform = cardSockets[socketSlot],
            associatedCard = null,
            isAvailable = true
        };

        socketData[socketSlot] = updatedSocketData;

    }

    public bool IsSocketAvailable(int socketSlot)
    {
        return socketData[socketSlot].isAvailable;
    }
}
