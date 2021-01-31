using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlay : MonoBehaviour
{

    public float step = 5f;

    SpriteRenderer thisSprite;
    float offset = 0f;

    public float modValue = 4.84f;

    public float lerpSpeed = 10f;
    float alpha = 0f;

    public bool isActive = false;


    // Start is called before the first frame update
    void Start()
    {

        thisSprite = GetComponent<SpriteRenderer>();
        thisSprite.color = new Color(1f, 1f, 1f, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            alpha = Mathf.Lerp(alpha, 1f, lerpSpeed * Time.deltaTime);
            thisSprite.color = new Color(1f, 1f, 1f, alpha); 

            offset = (offset + Time.deltaTime) % modValue;
            thisSprite.size = new Vector2(19.2f + offset, 10.8f);
        }
        else
        {
            alpha = Mathf.Lerp(alpha, 0f, lerpSpeed * Time.deltaTime);
            thisSprite.color = new Color(1f, 1f, 1f, alpha);
        }
        
    }

    public void Activate()
    {
        isActive = true;
    }
}
