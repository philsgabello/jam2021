using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationLibrary : MonoBehaviour
{

    AudioClip currentSoundToRandomize;
    List<AudioSource> audioSources = new List<AudioSource>();

    // Start is called before the first frame update
    void Start()
    {
        audioSources.Add(this.gameObject.AddComponent<AudioSource>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    AudioSource AddAudioSource()
    {
        AudioSource aSource = this.gameObject.AddComponent<AudioSource>();
        aSource.playOnAwake = false;
        aSource.loop = false;
        audioSources.Add(aSource);

        return aSource;
        
    }

    public void PlaySound(AudioClip clip)
    {
        currentSoundToRandomize = clip;

        bool isAudioSourcAvailable = false;
        int count = 0;

        AudioSource aSource;

        foreach (AudioSource a in audioSources)
        {
            if (!a.isPlaying)
            {
                isAudioSourcAvailable = true;
                break;
            }
            count++;
        }

        if (!isAudioSourcAvailable)
        {
            aSource = AddAudioSource();
        }
        else
        {
            aSource = audioSources[count];
        }

        aSource.clip = clip;
        aSource.Play();

    }

    public void PlaySoundRandom(AudioClip clip)
    {
        if(currentSoundToRandomize != clip)
        {
            currentSoundToRandomize = clip;
            PlaySound(clip);
            return;
        }
        if(Random.Range(0f, 1f) <= .3f)
        {
            PlaySound(clip);
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
