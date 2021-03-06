using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    public static MainCharacter instance;

    Table table;

    public Animator animator;

    Card cardOnHand;

    public Transform handSocket;

    GameManager.GameplayState nextState;

    bool isRegistered = false;

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

        EventManager.CharacterAnimationTrigger += SetAnimatorTrigger;

    }

    // Update is called once per frame
    void Update()
    {
        cardOnHand.SetLayer(18);
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

    public void ReleaseAllSlots()
    {
        for(int i = 0;i < 10; i++)
        {
            table.AppendSlotRemoval(i);
        }
        table.ReleaseSlots();
    }

    public void ReleaseSlot(int i)
    {
        table.AppendSlotRemoval(i);
        
        
    }

    public void SetupCardOnHand(char c)
    {
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

    public void RegisterNewGameplayState(GameManager.GameplayState state)
    {
        
        nextState = state;
        isRegistered = true;
    }
    public void ApplyState()
    {
        if (isRegistered)
        {
            GameManager.instance.SetGameplayPhase(nextState);
            isRegistered = false;
        }
        
    }

    public void SetColorOnCardOnTable()
    {
        
        table.SetColorOnCard();
    }
    public void ResetColorOnCardOnTable()
    {

        table.ResetColorOnCard();
    }

    public void ChangeColorOnCard(Card.CardType type)
    {
        cardOnHand.SetColor(type);
    }
}
