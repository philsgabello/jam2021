using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatGenerator : MonoBehaviour
{
    
    public Sprite[] catBodies;
    public Sprite[] catOutlines;
    public Sprite[] patterns;

    public Color[] primaries;
    public Color[] secundaries;

    public SpriteRenderer Body;
    public SpriteRenderer Pattern;
    public SpriteRenderer outline;
    public SpriteMask patternMask;

    public bool showCase;
    public float showcaseTime = 1f;
    float timerValue = 0f;

    // Start is called before the first frame update
    void Start()
    {
        GenerateNewCat();
    }

    // Update is called once per frame
    void Update()
    {
        timerValue += Time.deltaTime;
        if (showCase && timerValue >= showcaseTime)
        {
            GenerateNewCat();
            timerValue = 0f;
        }
    }

    public void GenerateNewCat()
    {
        int randomCatSelector = UnityEngine.Random.Range(0, catBodies.Length);
        Sprite randomBody = catBodies[randomCatSelector];
        Sprite outlineImage = catOutlines[randomCatSelector];
        Sprite randomPattern = patterns[UnityEngine.Random.Range(0, patterns.Length)];

        Color primaryColor = primaries[UnityEngine.Random.Range(0, primaries.Length)];
        Color secundaryColor = secundaries[UnityEngine.Random.Range(0, secundaries.Length)];

        Body.sprite = randomBody;
        Body.color = primaryColor;

        Pattern.sprite = randomPattern;
        Pattern.color = secundaryColor;
        patternMask.sprite = randomBody;

        outline.sprite = outlineImage;
    }
}
