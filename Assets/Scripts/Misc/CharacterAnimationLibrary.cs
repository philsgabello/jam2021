using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationLibrary : MonoBehaviour
{

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(AudioClip clip)
    {
        MainCharacter.instance.PlaySound(clip);

    }

    public void ApplyCardSlots()
    {
        MainCharacter.instance.ApplyCardSlots();
    }

    public void SetCardOnHandVisible()
    {
        MainCharacter.instance.SetCardOnHandVisible(true);
    }
    public void SetCardOnHandInvisible()
    {
        MainCharacter.instance.SetCardOnHandVisible(false);
    }
}
