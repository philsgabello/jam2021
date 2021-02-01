using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FileHandler : MonoBehaviour
{
    List<string> names = new List<string>();
    List<string> locations = new List<string>();

    string namesFilePath = "Assets/Resources/names.txt";
    string locationsFilePath = "Assets/Resources/locations.txt";

    string LetterImagesPath = "Assets/Resources/Letters/";
    Dictionary<char, string> letterPaths = new Dictionary<char, string>();

    public List<string> GetListOfNames()
    {
        if(names.Count != 0)
        {
            return names;
        }

        TextAsset txt = (TextAsset)Resources.Load("names", typeof(TextAsset));
        
        string fullText = txt.text;
        Debug.Log(fullText);

        List<string> tempList = new List<string>(fullText.Split('\n'));


        tempList.RemoveAt(tempList.Count - 1);

        for (int i = 0; i < tempList.Count; i++)
        {
            tempList[i] = tempList[i].Remove(tempList[i].Length - 1);
        }

        Debug.Log(tempList[tempList.Count -1]);

        names = tempList;

        return names;
    }

    public List<string> GetListOfLocations()
    {
        if (locations.Count != 0)
        {
            return locations;
        }

        TextAsset txt = (TextAsset)Resources.Load("locations", typeof(TextAsset));

        string fullText = txt.text;

        List<string> tempList = new List<string>(fullText.Split('\n'));

        tempList.RemoveAt(tempList.Count - 1);

        for (int i = 0; i < tempList.Count; i++)
        {

            tempList[i] = tempList[i].Remove(tempList[i].Length - 1);
        }

        Debug.Log(tempList[tempList.Count - 1]);


        locations = tempList;



        return locations;
    }

    public Dictionary<char, string> GetListOfLetterPaths()
    {
        if(letterPaths.Count == 26)
        {
            return letterPaths;
        }

        char firstChar = (char)65;

        for(int i = 0; i < 26; i++)
        {
            char tempCurrentChar = (char)((int)firstChar + 1);
            string tempPath = LetterImagesPath + tempCurrentChar + ".png";
            letterPaths.Add(tempCurrentChar, tempPath);
        }

        return letterPaths;
        
    }


}
