using System.Collections;
using System.Collections.Generic;
using System.IO;
public class FileHandler
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

        StreamReader reader = new StreamReader(namesFilePath);
        string fullText = reader.ReadToEnd();

        List<string> tempList = new List<string>();
        tempList.AddRange(fullText.Split('\n'));


        for(int i = 0; i < tempList.Count; i++)
        {
            tempList[i] = tempList[i].Remove(tempList[i].Length - 1);
        }

        names = tempList;

        return names;
    }

    public List<string> GetListOfLocations()
    {
        if (locations.Count != 0)
        {
            return locations;
        }

        StreamReader reader = new StreamReader(locationsFilePath);
        string fullText = reader.ReadToEnd();

        List<string> tempList = new List<string>();
        tempList.AddRange(fullText.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries));

        for (int i = 0; i < tempList.Count; i++)
        {
            tempList[i] = tempList[i].Remove(tempList[i].Length - 1);
        }


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
