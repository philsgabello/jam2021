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

    // Start is called before the first frame update
    void Start()
    {
        
        if(instance == null)
        {
            instance = this;
        }

        fileHandler = new FileHandler();

        StartCoroutine(StartGamestate(1f));
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
                break;

            case GameState.GameOver:
                break;

            default:
                break;
        }
    }

    void ProcessGameplay()
    {

        Debug.Log("Process Gameplay with State = " + gameplayPhase);
        switch (gameplayPhase)
        {
            case GameplayState.Idle:
                if (shouldRemove)
                {
                    shouldRemove = false;
                    gameplayPhase = GameplayState.Remove;
                }
                else
                {
                    shouldRemove = true;
                    gameplayPhase = GameplayState.Add;
                }
                break;
            case GameplayState.Remove:
                gameplayPhase = GameplayState.WaitingPlayerRelease;
                StartCoroutine(PickWrongCard(1f));
                break;
            case GameplayState.WaitingPlayerRelease:
                if (CheckContained(ExtractWord(currentGameplayString), InputManager.instance.CurrentInput))
                {
                    Debug.Log("Correct");
                    //The game starts
                    //TODO: GFX
                    gameplayPhase = GameplayState.Add;
                }
                break;
            case GameplayState.Add:
                break;
            case GameplayState.WaitingPlayerPress:
                break;
        } 
    }

    public void BeginNewGameInstance()
    {

        currentCatName = SelectNewName().ToUpper();
        currentLocationName = SelectNewLocation().ToUpper();

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
        if(currentGameplayString == ExtractWord(currentGameplayString))
        {
            Debug.Log("Finito");
            return;
        }

        char guess;
        int analyzedPosition;

        do
        {
            analyzedPosition = UnityEngine.Random.Range(0, currentLocationName.Length - 1);
            guess = currentGameplayString[analyzedPosition];

        } while (guess == currentLocationName[analyzedPosition]);

        Debug.Log(guess);
        if (guess == ' ')
        {
            gameplayPhase = GameplayState.Add;
        }
        else
        {
            currentGameplayString = currentGameplayString.Remove(analyzedPosition, 1).Insert(analyzedPosition, " ");
            Debug.Log(currentGameplayString);
            MainCharacter.instance.ReleaseSlot(analyzedPosition, guess);
            EventManager.instance.SetCharacterAnimation("takeCard");
            gameplayPhase = GameplayState.WaitingPlayerRelease;
        }
        
        
    }

    string ExtractWord(string s)
    {
        return s.Replace(" ", "");
    }
}
