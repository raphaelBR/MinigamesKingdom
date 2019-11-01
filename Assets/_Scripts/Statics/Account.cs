using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Unlocks
{
    public List<string> packs = new List<string>() { "food" };
    public List<string> skins = new List<string>() { "Default" };
    public List<string> fonts = new List<string>() { "Asap" };
}

/// <summary>
/// Holds the player account purchases (will probably fuse with Progress later on)
/// </summary>
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
        //var t = Resources.Load<TextAsset>("unlocks");
        //unlocks = JsonUtility.FromJson<Unlocks>(t.text);
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

    public static void Unlock(UnlockType type, string id)
    {
        switch (type)
        {
            case UnlockType.Pack:
                if (unlocks.packs.Contains(id) == false)
                {
                    unlocks.packs.Add(id);
                }
                break;
            case UnlockType.Skin:
                if (unlocks.skins.Contains(id) == false)
                {
                    unlocks.skins.Add(id);
                }
                break;
            case UnlockType.Font:
                if (unlocks.fonts.Contains(id) == false)
                {
                    unlocks.fonts.Add(id);
                }
                break;
            default:
                break;
        }
        Save();
    }

    public static bool IsUnlocked(UnlockType type, string id)
    {

        switch (type)
        {
            case UnlockType.Pack:
                return unlocks.packs.Contains(id);
            case UnlockType.Skin:
                return unlocks.skins.Contains(id);
            case UnlockType.Font:
                return unlocks.fonts.Contains(id);
            default:
                return false;
        }
    }
}
