using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public enum GameState
    {
        Idle, WaitFirstInput, Gameplay, GameOver, Complete
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

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        fileHandler = new FileHandler();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.Idle:
                break;

            case GameState.WaitFirstInput:
                if (CheckAnagram(InputManager.instance.CurrentInput, currentCatName))
                {
                    //The game starts
                    //TODO: GFX
                    SetGameState(GameState.Gameplay);
                }
                break;

            case GameState.Gameplay:
                break;

            case GameState.Complete:
                break;

            case GameState.GameOver:
                break;

            default:
                break;
        }
    }

    public void BeginNewGameInstance()
    {
        currentCatName = SelectNewName();
        currentLocationName = SelectNewLocation();

        debugText.text = currentCatName;
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

    public void Advance()
    {
        //Check if finished and correct
            //END GAME INSTANCE

        //next letter
        //Update Input and gfx
    }

    public void Regress()
    {
        //Check if GAME OVER
            //END GAME INSTANCE

        //retry
        //Reload state
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

}
