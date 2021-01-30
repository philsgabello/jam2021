using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public SpriteRenderer letterSR;

    Transform assignedSlot;

    public bool isAvailable = true;

    //PLACEHOLDER
    public TextMesh textMesh;

    char letter;
    Sprite letterSprite;

    public Card(char letter)
    {
        Setup(letter);
    }

    public Card()
    {
        //Setup('A');
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup()
    {
        //letterSR.sprite = letterSprite;
        textMesh.text = letter.ToString();
    }
    public void Setup(char letter)
    {
        this.letter = letter;
        //letterSprite = GameManager.instance.GetLetterSprite(letter);

        Setup();
    }

    public void AssignSlot(Transform t)
    {
        assignedSlot = t;
        this.transform.SetParent(assignedSlot);
        this.transform.localPosition = Vector3.zero;
        isAvailable = false;

    }

    public void ClearSlot()
    {
        AssignSlot(PoolingManager.instance.CardSocket);
        isAvailable = true;
        
    }

    public void BookCard()
    {
        isAvailable = false;
    }

    public void SetVisibility(bool b)
    {
        textMesh.GetComponent<MeshRenderer>().enabled = b;
        letterSR.enabled = b;
    }
}
