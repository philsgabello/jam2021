using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationLibrary : MonoBehaviour
{

    AudioClip currentSoundToRandomize;

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

    public void PlaySoundRandom(AudioClip clip)
    {
        if(currentSoundToRandomize != clip)
        {
            currentSoundToRandomize = clip;
            MainCharacter.instance.PlaySound(clip);
            return;
        }
        if(Random.Range(0f, 1f) <= .3f)
        {
            MainCharacter.instance.PlaySound(clip);
        }
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

    public void ApplyGameplayState()
    {
        MainCharacter.instance.ApplyState();
    }
}
