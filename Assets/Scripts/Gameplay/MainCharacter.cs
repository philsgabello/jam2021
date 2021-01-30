using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    public static MainCharacter instance;

    AudioSource audioSource;
    Table table;

    public Animator animator;

    Card cardOnHand;

    public Transform handSocket;

    // Start is called before the first frame update
    void Start()
    {
        cardOnHand = PoolingManager.instance.GetAvailableCard();
        cardOnHand.SetVisibility(false);
        cardOnHand.AssignSlot(handSocket);


        table = FindObjectOfType<Table>();

        if(instance == null)
        {
            instance = this;
        }

        audioSource = this.GetComponent<AudioSource>();
        EventManager.CharacterAnimationTrigger += SetAnimatorTrigger;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(AudioClip audio)
    {
        audioSource.Stop();
        audioSource.clip = audio;
        audioSource.Play();

    }

    public void SetAnimatorTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public void NewWordBooking(string newWord)
    {
        for(int i= 0; i < newWord.Length; i++)
        {
            BookCardForSlot(i, newWord[i]);
        }

    }

    public void BookCardForSlot(int i, char c)
    {
        Card card = PoolingManager.instance.GetAvailableCard();
        card.Setup(c);
        if (!table.IsSlotAvailable(i))
        {
            Debug.LogError("ERROR: The slot is NOT available");
            return;
        }
        table.BookSlot(i, card);
    }

    public void ReleaseSlot(int i, char c)
    {
        table.AppendSlotRemoval(i);
        cardOnHand.Setup(c);
        
    }

    public void ApplyCardSlots()
    {
        table.ApplyCardsToSlot();
        table.ReleaseSlots();
    }

    public void SetCardOnHandVisible(bool isVisible)
    {
        cardOnHand.SetVisibility(isVisible);
    }
}
