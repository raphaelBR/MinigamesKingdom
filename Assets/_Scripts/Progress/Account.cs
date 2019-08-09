using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Unlocks
{
    public List<string> packs = new List<string>() { "food", "colors" };
}

static class Account
{
    public static Unlocks unlocks = new Unlocks();
    static string UnlocksPath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, "unlocks.json");
        }
        set { }
    }


    static Account()
    {
        Load();
    }

    public static void Save()
    {
        string jsonString = JsonUtility.ToJson(unlocks);

        using (StreamWriter streamWriter = File.CreateText(UnlocksPath))
        {
            streamWriter.Write(jsonString);
        }
    }

    public static void Load()
    {
        if (File.Exists(UnlocksPath))
        {
            using (StreamReader streamReader = File.OpenText(UnlocksPath))
            {
                string jsonString = streamReader.ReadToEnd();
                unlocks = JsonUtility.FromJson<Unlocks>(jsonString);
            }
        }
        else
        {
            Save();
        }
    }
}
