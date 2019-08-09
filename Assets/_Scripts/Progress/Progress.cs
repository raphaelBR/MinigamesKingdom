using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using RotaryHeart.Lib.SerializableDictionary;

[Serializable]
public class Experience
{
    [SerializeField]
    int level = 1;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            if (value < 1)
            {
                level = 1;
            }
            else
            {
                level = value;
            }
        }
    }

    [SerializeField]
    int xp = 0;
    public int Xp
    {
        get
        {
            return xp;
        }
        set
        {
            if (value < 0)
            {
                xp = 0;
            }
            else
            {
                xp = value;
            }
        }
    }
}

[Serializable]
public class ExperienceDictionary : SerializableDictionaryBase<string, Experience> { }

[Serializable]
public class FavoritesDictionary : SerializableDictionaryBase<string, bool> { }

[Serializable]
public class Progression
{
    [SerializeField]
    public FavoritesDictionary wordsFavorites = new FavoritesDictionary();
    [SerializeField]
    public ExperienceDictionary wordsMastery = new ExperienceDictionary();
    [SerializeField]
    public Experience userMastery = new Experience();
    [SerializeField]
    public int pointsA = 0;
    [SerializeField]
    public int pointsB = 0;
    [SerializeField]
    public int pointsC = 0;
}

[Serializable]
public class LevelCap
{
    public int[] wordsCap = new int[5]
    {
        10,
        20,
        30,
        40,
        50
    };
    public int[] userCap = new int[5]
    {
        100,
        200,
        300,
        400,
        500
    };
}


static class Progress
{
    public static Progression progress = new Progression();
    public static LevelCap cap = new LevelCap();
    static string ProgressPath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, Parameters.Foreign.ToString().ToLower() + "_progress.json");
        }
        set { }
    }

    static Progress()
    {
        Load();
    }

    public static void Wipe()
    {
        List<Languages> langList = Enum.GetValues(typeof(Languages)).Cast<Languages>().ToList();
        foreach (var lang in langList)
        {
            string path = Path.Combine(Application.persistentDataPath, lang.ToString().ToLower() + "_progress.json");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        progress = new Progression();
    }

    public static void Save()
    {
        string jsonString = JsonUtility.ToJson(progress);

        using (StreamWriter streamWriter = File.CreateText(ProgressPath))
        {
            streamWriter.Write(jsonString);
        }
    }

    public static void Load()
    {
        if (File.Exists(ProgressPath))
        {
            using (StreamReader streamReader = File.OpenText(ProgressPath))
            {
                string jsonString = streamReader.ReadToEnd();
                progress = JsonUtility.FromJson<Progression>(jsonString);
            }
        }
        else
        {
            progress = new Progression();
            Save();
        }

    }

    public static void GainXP(int amount)
    {
        if (GetExp().Level >= cap.userCap.Length)
        {
            return;
        }

        progress.userMastery.Xp += amount;
        while (progress.userMastery.Xp >= GetCap())
        {
            if (progress.userMastery.Level >= cap.userCap.Length)
            {
                break;
            }
            progress.userMastery.Xp -= GetCap();
            progress.userMastery.Level++;
        }
    }

    public static void GainXP(int amount, string word)
    {
        if (GetExp(word).Level >= cap.wordsCap.Length)
        {
            return;
        }

        progress.wordsMastery[word].Xp += amount;
        while (progress.wordsMastery[word].Xp >= GetCap(word))
        {
            if (progress.wordsMastery[word].Level >= cap.wordsCap.Length)
            {
                break;
            }
            progress.wordsMastery[word].Xp -= GetCap(word);
            progress.wordsMastery[word].Level++;
        }
    }

    public static void GainPoints(int amount, int type = 0)
    {
        switch (type)
        {
            case 1:
                progress.pointsA += amount;
                break;
            case 2:
                progress.pointsB += amount;
                break;
            case 3:
                progress.pointsC += amount;
                break;
            default:
                break;
        }
    }

    public static int GetCap()
    {
        return cap.userCap[Mathf.Min(GetExp().Level, cap.userCap.Length) - 1];
    }

    public static int GetCap(string word)
    {
        return cap.wordsCap[Mathf.Min(GetExp(word).Level, cap.wordsCap.Length) - 1];
    }

    public static Experience GetExp()
    {
        return progress.userMastery;
    }

    public static Experience GetExp(string word)
    {
        if (progress.wordsMastery.ContainsKey(word) == false)
        {
            progress.wordsMastery.Add(word, new Experience());
        }
        return progress.wordsMastery[word];
    }


    public static bool IsFavorite(string word)
    {
        if (progress.wordsFavorites.ContainsKey(word) == false)
        {
            SetFavorite(word, false);
        }
        return progress.wordsFavorites[word];
    }

    public static void SetFavorite(string word, bool set)
    {
        if (progress.wordsFavorites.ContainsKey(word) == false)
        {
            progress.wordsFavorites.Add(word, set);
        }
        else
        {
            progress.wordsFavorites[word] = set;
        }
        Save();
    }

}
