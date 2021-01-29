using System.Collections;
using System.Collections.Generic;
using System.IO;
public class FileHandler
{
    List<string> names = new List<string>();
    List<string> locations = new List<string>();

    string namesFilePath = "Assets/Resources/names.txt";
    string locationsFilePath = "Assets/Resources/loactions.txt";

    public List<string> GetListOfNames()
    {
        if(names.Count != 0)
        {
            return names;
        }

        StreamReader reader = new StreamReader(namesFilePath);
        string fullText = reader.ReadToEnd();

        List<string> tempList = new List<string>();
        tempList.AddRange(fullText.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries));

        //Controlling the list

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

        //Controlling the list

        locations = tempList;

        return locations;
    }


}
