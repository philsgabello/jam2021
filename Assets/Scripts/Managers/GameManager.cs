using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    FileHandler fileHandler;

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
        
    }

    public void BeginNewGameInstance()
    {
        //TODO: SELECT NAME WORD
        //TODO: SELECT LOCATION
        //TODO: CALL DISPLAY NAME
        //TODO: ENABLE USER INPUT
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


}
