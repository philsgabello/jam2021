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
        Idle, WaitForFirstInput, WaitForCorrection, Gameplay, GameOver, Complete
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

    bool firstTime = true;

    private int lives = 3;

    private GameState gameState = GameState.Idle;

    public static GameManager instance;

    public FileHandler fileHandler;

    string currentCatName;
    string currentLocationName;

    string currentGameplayString;
    string previousGameplayString;

    GameplayState gameplayPhase = GameplayState.Idle;
    bool shouldRemove = true;

    int thisAnalyzedPosition;

    public Animator masterAnim;

    public Overlay overlay;

    public CatGenerator cat;

    public EndBackground endBkg;



    // Start is called before the first frame update
    void Start()
    {
        
        if(instance == null)
        {
            instance = this;
        }

        

        StartCoroutine(StartGamestate(3f));
    }

    // Update is called once per frame
    void Update()
    {


        switch (gameState)
        {
            case GameState.Idle:
                break;

            case GameState.WaitForFirstInput:
                if (CheckContained(ExtractWord(currentGameplayString), InputManager.instance.CurrentInput))
                {
                    SetGameState(GameState.Gameplay);
                }
                break;

            case GameState.Gameplay:
                ProcessGameplay();
                break;

            case GameState.WaitForCorrection:
                if (CheckContained(ExtractWord(currentGameplayString), InputManager.instance.CurrentInput))
                {
                    string triggerName = (shouldRemove) ? "drawCard" : "takeCard";
                    EventManager.instance.SetCharacterAnimation(triggerName);
                    SetGameState(GameState.Gameplay);
                }
                break;

            case GameState.Complete:
                endBkg.SetBackground(GetLocationByInput(ExtractWord(currentGameplayString)));
                MainCharacter.instance.ReleaseAllSlots();
                MainCharacter.instance.SetCardOnHandVisible(false);
                EventManager.instance.SetCharacterAnimation("win");
                SetGameState(GameState.Idle);
                StartCoroutine(WinState(4f));
                StartCoroutine(StartNewGameFromWin(10f));
                break;

            case GameState.GameOver:
                overlay.isActive = false;
                MainCharacter.instance.ReleaseAllSlots();
                MainCharacter.instance.SetCardOnHandVisible(false);
                Debug.LogWarning("GAME OVER");
                EventManager.instance.SetCharacterAnimation("gameOver");
                SetGameState(GameState.Idle);
                StartCoroutine(StartNewGameFromGameOver(4f));
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
                    //MainCharacter.instance.RegisterNewGameplayState(GameplayState.Remove);
                    gameplayPhase = GameplayState.Remove;
                }
                else
                {
                    Debug.LogWarning("Gameplay: " + "Pass To Add");
                    //MainCharacter.instance.RegisterNewGameplayState(GameplayState.Add);
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
                if (!CheckContained(ExtractWord(previousGameplayString), InputManager.instance.CurrentInput) && !CheckContained(ExtractWord(currentGameplayString), InputManager.instance.CurrentInput))
                {
                    if(lives != 0)
                    {
                        RemoveLife();
                    }
                    else
                    {
                        
                        SetGameState(GameState.GameOver);
                    }
                    
                    
                }
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
                if (!CheckContained(ExtractWord(previousGameplayString), InputManager.instance.CurrentInput) && !CheckContained(ExtractWord(currentGameplayString), InputManager.instance.CurrentInput))
                {
                    if (lives != 0)
                    {
                        RemoveLife();
                    }
                    else
                    {
                        SetGameState(GameState.GameOver);
                    }
                }
                if (CheckContained(ExtractWord(currentGameplayString), InputManager.instance.CurrentInput))
                {
                    EventManager.instance.SetCharacterAnimation("setCard");
                    gameplayPhase = GameplayState.Idle;
                }
                break;
        } 
    }

    private void RemoveLife()
    {
        lives--;
        Debug.LogWarning("Remaining Lives: " + lives);
        SetGameState(GameState.WaitForCorrection);
        EventManager.instance.SetCharacterAnimation("error");
        LifeBar.instance.RemoveLife();
    }

    public void BeginNewGameInstance()
    {
        shouldRemove = true;
        Invoke("GenerateCat", 3f);
        lives = 3;
        LifeBar.instance.Reset();

        currentCatName = SelectNewName().ToUpper();
        overlay.isActive = true;

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

        SetGameState(GameState.WaitForFirstInput);

    }

    private void SetGameState(GameState newGameState)
    {
        gameState = newGameState;
    }

    private string SelectNewName(Difficulty difficulty = Difficulty.Easy)
    {
        List<string> temp = fileHandler.GetListOfNames();

        string candidate = temp[UnityEngine.Random.Range(0, temp.Count)];

        return candidate;
    }

    private string SelectNewLocation(Difficulty difficulty = Difficulty.Easy)
    {

        List<string> temp = fileHandler.GetListOfLocations();
        

        string candidate = temp[UnityEngine.Random.Range(0, temp.Count)];

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
            //MainCharacter.instance.RegisterNewGameplayState(GameplayState.Idle);
            gameplayPhase = GameplayState.Idle;
        }
        else
        {
            previousGameplayString = currentGameplayString;
            currentGameplayString = currentGameplayString.Remove(analyzedPosition, 1).Insert(analyzedPosition, " ");
            
            Debug.LogWarning("Gameplay: " + "Removed " + guess + " - New string is _" + ExtractWord(currentGameplayString) + "_" );

            Card.CardType cardType = currentGameplayString.Contains(guess.ToString()) ? Card.CardType.AlreasyPresent : Card.CardType.Remove;

            MainCharacter.instance.ReleaseSlot(analyzedPosition);
            MainCharacter.instance.SetupCardOnHand(guess);
            MainCharacter.instance.ChangeColorOnCard(cardType);
            EventManager.instance.SetCharacterAnimation("takeCard");

            MainCharacter.instance.RegisterNewGameplayState(GameplayState.WaitingPlayerRelease);
            //gameplayPhase = GameplayState.WaitingPlayerRelease;
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

        Card.CardType cardType = currentGameplayString.Contains(substitute.ToString()) ? Card.CardType.AlreasyPresent : Card.CardType.Add;

        previousGameplayString = currentGameplayString;
        currentGameplayString = currentGameplayString.Remove(thisAnalyzedPosition, 1).Insert(thisAnalyzedPosition, substitute.ToString());
        
        Debug.LogWarning("Gameplay: " + "Added " + substitute + " - New string is _" + ExtractWord(currentGameplayString) + "_");

        

        MainCharacter.instance.BookCardForSlot(thisAnalyzedPosition, substitute);
        MainCharacter.instance.SetupCardOnHand(substitute);
        MainCharacter.instance.ChangeColorOnCard(cardType);
        EventManager.instance.SetCharacterAnimation("drawCard");

        MainCharacter.instance.RegisterNewGameplayState(GameplayState.WaitingPlayerPress);
        //gameplayPhase = GameplayState.WaitingPlayerPress;

    }

    string ExtractWord(string s)
    {
        return s.Replace(" ", "");
    }

    public void SetGameplayPhase(GameplayState newPhase)
    {
        gameplayPhase = newPhase;
    }

    IEnumerator WinState(float delay)
    {
        yield return new WaitForSeconds(delay);
        WinState_Internal();
    }

    void WinState_Internal()
    {
        masterAnim.SetTrigger("exit");
    }


    IEnumerator StartNewGameFromGameOver(float delay)
    {
        yield return new WaitForSeconds(delay);

        EventManager.instance.SetCharacterAnimation("reset");

        BeginNewGameInstance();
    }

    IEnumerator StartNewGameFromWin(float delay)
    {
        yield return new WaitForSeconds(delay);

        EventManager.instance.SetCharacterAnimation("reset");
        masterAnim.SetTrigger("reset");

        BeginNewGameInstance();
    }

    void GenerateCat()
    {
        cat.GenerateNewCat();
    }

    EndBackground.Location GetLocationByInput(string s)
    {
        if(s == "CASTLE")
        {
            return EndBackground.Location.Castle;
        }
        else if (s == "THEFACTORY")
        {
            return EndBackground.Location.Factory;
        }
        else if (s == "INSPACE")
        {
            return EndBackground.Location.Space;
        }
        else if (s == "THEBANK")
        {
            return EndBackground.Location.Bank;
        }
        else if (s == "HOSPITAL")
        {
            return EndBackground.Location.Hospital;
        }
        else if (s == "SUPERMAX")
        {
            return EndBackground.Location.Prison;
        }
        return EndBackground.Location.Space;
    }
}
