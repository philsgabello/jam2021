using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour
{

    public Animator[] anims;

    public static LifeBar instance;

    int count = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveLife()
    {
        if (count < anims.Length)
        {
            anims[count].SetTrigger("fall");
            count++;
        }
        
    }

    public void Reset()
    {
        count = 0;

        foreach(Animator a in anims)
        {
            a.SetTrigger("reset");
        }
    }


}
