using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{

    public Transform CardSocket
    {
        get { return cardsSocket; }
    }

    public GameObject cardPrefab;

    Transform cardsSocket;


    List<Card> cardsPool = new List<Card>();
    int maxCardPoolSize = 15;

    public static PoolingManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        cardsSocket = (new GameObject("Cards Socket")).transform;

        cardsSocket.SetParent(this.transform);
        cardsSocket.localPosition = Vector3.zero;


        for (int i = 0; i < maxCardPoolSize; i++)
        {
            GameObject newObject = GameObject.Instantiate(cardPrefab);
            cardsPool.Add(newObject.GetComponentInChildren<Card>());
            cardsPool[i].ClearSlot();
        }
    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Card GetAvailableCard()
    {
        foreach(Card c in cardsPool)
        {
            if (c.isAvailable)
            {
                c.BookCard();
                return c;
            }
        }

        GameObject newObject = GameObject.Instantiate(cardPrefab);
        cardsPool.Add(newObject.GetComponentInChildren<Card>());

        return newObject.GetComponent<Card>();

    }
}
