using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlay : MonoBehaviour
{

    public float step = 5f;

    SpriteRenderer thisSprite;
    // Start is called before the first frame update
    void Start()
    {
        thisSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        thisSprite.size += new Vector2(step * Time.deltaTime, 0f);
    }
}
