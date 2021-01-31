using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBackground : MonoBehaviour
{


    public Sprite[] sprites;
    public enum Location
    {
        Bank, Castle, Factory, Hospital, Prison, Space
    }

    Dictionary<Location, Sprite> dictionary = new Dictionary<Location, Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < sprites.Length; i++)
        {
            dictionary.Add((Location)i, sprites[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBackground(Location loc)
    {
        GetComponent<SpriteRenderer>().sprite = dictionary[loc];
    }
}
