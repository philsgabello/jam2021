using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public SpriteRenderer letterSR;

    Transform assignedSlot;

    public bool isAvailable = true;
    public SpriteRenderer borderRenderer;

    //PLACEHOLDER
    public TextMesh textMesh;

    char letter;
    Sprite letterSprite;

    public Color baseColor = new Color(188f / 255f, 39f / 255f, 240f / 255f);
    public Color addColor = new Color(38f/255f, 206f / 255f, 75f / 255f);
    public Color removeColor = new Color(206f / 255f, 38f / 255f, 63f / 255f);
    public Color alreadyColor = new Color(104f / 255f, 199f / 255f, 191f / 255f);

    Color currentColor;



    public enum CardType
    {
        Base, Add, Remove, AlreasyPresent
    }

    Dictionary<CardType, Color> colorDictionary = new Dictionary<CardType, Color>();

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
        colorDictionary.Add(CardType.Base, baseColor);
        colorDictionary.Add(CardType.Add, addColor);
        colorDictionary.Add(CardType.Remove, removeColor);
        colorDictionary.Add(CardType.AlreasyPresent, alreadyColor);

        currentColor = baseColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup()
    {
        //letterSR.sprite = letterSprite;
        textMesh.text = letter.ToString();
        borderRenderer.color = currentColor;
    }
    public void Setup(char letter, CardType cType = CardType.Base)
    {
        this.letter = letter;
        currentColor = colorDictionary[cType];
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
        borderRenderer.enabled = b;

    }

    public void SetColor(CardType cType)
    {
        currentColor = colorDictionary[cType];
        borderRenderer.color = currentColor;
    }

    public void SetLayer(int layer)
    {
        letterSR.sortingOrder = layer;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = layer + 2;
        borderRenderer.sortingOrder = layer + 1;
    }
}
