using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    //CHARACTER EVENTS
    public delegate void BaseTrigger(string eventName);
    public static event BaseTrigger CharacterAnimationTrigger;

    public static EventManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCharacterAnimation(string triggerName)
    {
        CharacterAnimationTrigger(triggerName);
    }
}
