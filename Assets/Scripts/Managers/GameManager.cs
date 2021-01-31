using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public enum GameState
    {
        Idle, WaitFirstInput, Gameplay, GameOver, Complete
    }

    public enum GameplayState
    {
        Idle, Remove, WaitingPlayerRelease, Add, WaitingPlayerPress
    }

    public enum Difficulty
    {
        Easy, Medium, Hard
    }

    public GameState State
    {
        get { return gameState; }

    }

    private GameState gameState = GameState.Idle;

    public Text debugText;

    public static GameManager instance;

    FileHandler fileHandler;

    string currentCatName;
    string currentLocationName;

    string currentGameplayString;

    GameplayState gameplayPhase = GameplayState.Idle;
    bool shouldRemove = true;

    int thisAnalyzedPosition;

    // Start is called before the first frame update
    void Start()
    {
        
        if(instance == null)
        {
            instance = this;
        }

        fileHandler = new FileHandler();

        StartCoroutine(StartGamestate(3f));
    }

    // Update is called once per frame
    void Update()
    {


        switch (gameState)
        {
            case GameState.Idle:
                break;

            case GameState.WaitFirstInput:
                if (CheckContained(ExtractWord(currentGameplayString), InputManager.instance.CurrentInput))
                {
                    //The game starts
                    //TODO: GFX
                    SetGameState(GameState.Gameplay);
                }
                break;

            case GameState.Gameplay:
                ProcessGameplay();
                break;

            case GameState.Complete:
                EventManager.instance.SetCharacterAnimation("win");
                SetGameState(GameState.Idle);
                break;

            case GameState.GameOver:
                break;

            default:
                break;
        }
    }

    void ProcessGameplay()
    {
        //Debug.Log(gameplayPhase);
        switch (gameplayPhase)
        {
            case GameplayState.Idle:
                if (shouldRemove)
                {
                    Debug.LogWarning("Gameplay: " + "Pass To Remove");
                    gameplayPhase = GameplayState.Remove;
                }
                else
                {
                    Debug.LogWarning("Gameplay: " + "Pass To Add");
                    gameplayPhase = GameplayState.Add;
                }
                break;
            case GameplayState.Remove:
                if (shouldRemove)
                {
                    shouldRemove = false;
                    StartCoroutine(PickWrongCard(1f));
                }
                break;
            case GameplayState.WaitingPlayerRelease:
                if (CheckContained(ExtractWord(currentGameplayString), InputManager.instance.CurrentInput))
                {
                    

                    EventManager.instance.SetCharacterAnimation("shuffleCard");
                    gameplayPhase = GameplayState.Idle;
                }
                break;
            case GameplayState.Add:
                if (!shouldRemove)
                {
                    shouldRemove = true;
                    StartCoroutine(DrawCard(1f));
                }
                break;
            case GameplayState.WaitingPlayerPress:
                if (CheckContained(ExtractWord(currentGameplayString), InputManager.instance.CurrentInput))
                {
                    EventManager.instance.SetCharacterAnimation("setCard");
                    gameplayPhase = GameplayState.Idle;
                }
                break;
        } 
    }

    public void BeginNewGameInstance()
    {

        currentCatName = SelectNewName().ToUpper();
        string tempLocation;
        do
        {
            tempLocation = SelectNewLocation();
        } while(tempLocation.Length < currentCatName.Length);

        currentLocationName = tempLocation.ToUpper();

        Debug.LogWarning(currentCatName + "(" + currentCatName.Length + ")" + " -VS- " + currentLocationName + "(" + currentLocationName.Length + ")");

        currentGameplayString = currentCatName;
        for(int i = currentGameplayString.Length; i < 10; i++)
        {
            currentGameplayString = currentGameplayString + " ";
        }

        EventManager.instance.SetCharacterAnimation("drawCard");
        EventManager.instance.SetCharacterAnimation("setCard");

        MainCharacter.instance.NewWordBooking(ExtractWord(currentGameplayString));

        SetGameState(GameState.WaitFirstInput);

    }

    private void SetGameState(GameState newGameState)
    {
        gameState = newGameState;
    }

    private string SelectNewName(Difficulty difficulty = Difficulty.Easy)
    {
        List<string> temp = fileHandler.GetListOfNames();

        string candidate = temp[UnityEngine.Random.Range(0, temp.Count - 1)];

        return candidate;
    }

    private string SelectNewLocation(Difficulty difficulty = Difficulty.Easy)
    {
        List<string> temp = fileHandler.GetListOfLocations();

        string candidate = temp[UnityEngine.Random.Range(0, temp.Count - 1)];

        return candidate;
    }



    public void Regress()
    {
        //Check if GAME OVER
            //END GAME INSTANCE

        //retry
        //Reload state
    }

    private bool CheckContained(string A, string B)
    {

        foreach(char c in A.ToCharArray())
        {
            if (!B.Contains(c.ToString()))
            {
                return false;
            }

        }

        foreach (char c in B.ToCharArray())
        {
            if (!A.Contains(c.ToString()))
            {
                return false;
            }

        }
        return true;
    }

    private bool CheckAnagram(string A, string B)
    {
        int a = A.Length;
        int b = B.Length;

        if(a != b)
        {
            return false;
        }

        char[] arrayA = A.ToCharArray();
        char[] arrayB = B.ToCharArray();

        Array.Sort(arrayA);
        Array.Sort(arrayB);

        A = arrayA.ToString();
        B = arrayB.ToString();

        return A == B;


    }

    public Sprite GetLetterSprite(char letter)
    {
        string path = fileHandler.GetListOfLetterPaths()[letter];
        Sprite sprite = Resources.Load(path) as Sprite;
        return sprite;
    }


    IEnumerator StartGamestate(float delay)
    {

        yield return new WaitForSeconds(delay);

        BeginNewGameInstance();
    }

    IEnumerator PickWrongCard(float delay)
    {
        yield return new WaitForSeconds(delay);
        PickWrongCard_Internal();
    }

    void PickWrongCard_Internal()
    {
        Debug.Log("strings are same? " + (currentLocationName == ExtractWord(currentGameplayString)));
        if(currentLocationName == ExtractWord(currentGameplayString))
        {
            SetGameState(GameState.Complete);
            return;
        }

        char guess;
        int analyzedPosition;

        do
        {
            analyzedPosition = UnityEngine.Random.Range(0, currentLocationName.Length);
            guess = currentGameplayString[analyzedPosition];

        } while (guess == currentLocationName[analyzedPosition]);

        thisAnalyzedPosition = analyzedPosition;

        if (guess == ' ')
        {
            Debug.LogWarning("Gameplay: " + "No Previous Letter. Passing to ADD");
            gameplayPhase = GameplayState.Add;
        }
        else
        {
            
            currentGameplayString = currentGameplayString.Remove(analyzedPosition, 1).Insert(analyzedPosition, " ");
            
            Debug.LogWarning("Gameplay: " + "Removed " + guess + " - New string is _" + ExtractWord(currentGameplayString) + "_" );

            MainCharacter.instance.ReleaseSlot(analyzedPosition);
            MainCharacter.instance.SetupCardOnHand(guess);
            EventManager.instance.SetCharacterAnimation("takeCard");
            gameplayPhase = GameplayState.WaitingPlayerRelease;
        }
        
        
    }

    IEnumerator DrawCard(float delay)
    {
        yield return new WaitForSeconds(delay);
        DrawCard_Internal();
    }

    void DrawCard_Internal()
    {

        char substitute = currentLocationName[thisAnalyzedPosition];

        currentGameplayString = currentGameplayString.Remove(thisAnalyzedPosition, 1).Insert(thisAnalyzedPosition, substitute.ToString());
        
        Debug.LogWarning("Gameplay: " + "Added " + substitute + " - New string is _" + ExtractWord(currentGameplayString) + "_");

        MainCharacter.instance.BookCardForSlot(thisAnalyzedPosition, substitute);
        MainCharacter.instance.SetupCardOnHand(substitute);
        EventManager.instance.SetCharacterAnimation("drawCard");
        gameplayPhase = GameplayState.WaitingPlayerPress;

    }

    string ExtractWord(string s)
    {
        return s.Replace(" ", "");
    }

    public void SetGameplayPhase(GameplayState newPhase)
    {
        gameplayPhase = newPhase;
    }
}
