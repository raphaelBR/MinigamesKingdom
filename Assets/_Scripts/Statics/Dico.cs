using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#region SubClasses

public enum Languages
{
    Null,
    EN,
    FR,
    ES,
    IT,
    DE
}

[System.Serializable]
public class RarityData
{
    public RarityItem[] items;
}

[System.Serializable]
public class RarityItem
{
    public string key;
    public int value;
}

[System.Serializable]
public class LocalizationData
{
    public LocalizationItem[] items;
}

[System.Serializable]
public class LocalizationItem
{
    public string key;
    public string value;
}

public class Listing
{
    public List<string> packs = new List<string>();
}

public class DicoPackage
{
    public Sprite booster;
    public Sprite cardBack;
}

public class DicoItem
{
    public string localText;
    public string foreignText;
    public AudioClip foreignVoice;
    public Sprite picture;
    public int rarity;
    public string pack;
}

#endregion

/// <summary>
/// Holds the localized data for the words and packs.
/// </summary>
static class Dico
{
    public static Dictionary<string, DicoItem> dico = new Dictionary<string, DicoItem>();
    public static Dictionary<string, DicoItem> menu = new Dictionary<string, DicoItem>();
    public static Dictionary<string, DicoPackage> packs = new Dictionary<string, DicoPackage>();

    static System.Random rand = new System.Random();

    static Dico()
    {
        Parameters.Locale = Languages.FR;
        Parameters.Locale = Languages.EN;
        LoadPackages();
        LoadMenus();
    }

    public static void LoadMenus()
    {
        if (Parameters.Locale == Languages.Null)
        {
            Parameters.Locale = Languages.FR;
        }
        menu = LoadDictionary("menus");
    }

    public static void LoadPackages()
    {
        TextAsset asset = Resources.Load("Packs/listing") as TextAsset;
        if (asset == null)
        {
            throw new MissingReferenceException("Could not find the pack listing JSON file");
}
        else
        {
            packs.Clear();
            var temp = JsonUtility.FromJson<Listing>(asset.ToString());
            foreach (var s in temp.packs)
            {
                packs.Add(s, new DicoPackage
                {
                    booster = Resources.Load<Sprite>("Packs/Boosters/" + s),
                    cardBack = Resources.Load<Sprite>("Packs/Cards/" + s)
                });
            }
        }
    }

    public static void AddPack(string prefix)
    {
        Dictionary<string, DicoItem> temp = LoadDictionary(prefix);
        foreach (KeyValuePair<string, DicoItem> item in temp)
        {
            if (dico.ContainsKey(item.Key) == false)
            {
                dico.Add(item.Key, item.Value);
            }
        }
    }

    public static Dictionary<string, DicoItem> LoadDictionary(string prefix)
    {
        if (Parameters.Locale == Languages.Null)
        {
            Parameters.Locale = Languages.FR;
        }
        if (Parameters.Foreign == Languages.Null)
        {
            Parameters.Foreign = Languages.EN;
        }
        Dictionary<string, DicoItem> result = new Dictionary<string, DicoItem>();

        Dictionary<string, string> dicoLocal = LoadFile(prefix, Parameters.Locale);
        Dictionary<string, string> dicoForeign = LoadFile(prefix, Parameters.Foreign);

        if (dicoForeign == null)
        {
            dicoForeign = dicoLocal;
        }

        if (dicoForeign == null && dicoLocal == null)
        {
            throw new MissingReferenceException("Both of the JSON Dictionaries are missing");
        }
        
        if (dicoLocal.Count != dicoForeign.Count)
        {
            throw new System.Exception("JSON Dictionaries not of the same length");
        }

        Dictionary<string, int> dicoRarities = new Dictionary<string, int>();
        TextAsset asset = Resources.Load(prefix + "/" + prefix + "_rarity") as TextAsset;
        if (asset != null)
        {
            dicoRarities = JsonUtility.FromJson<RarityData>(asset.ToString()).items.ToDictionary(i => i.key, i => i.value);
        }

        string foreign;
        foreach (KeyValuePair<string, string> locale in dicoLocal)
        {
            if (dicoForeign.TryGetValue(locale.Key, out foreign) == false)
            {
                throw new KeyNotFoundException(locale.Key + " key do not match between the JSON Dictionaries");
            }
            int r = 1;
            dicoRarities.TryGetValue(locale.Key, out r);
            result.Add(locale.Key, new DicoItem
            {
                localText = locale.Value,
                foreignText = foreign,
                foreignVoice = Resources.Load<AudioClip>(prefix + "/sounds/" + locale.Key),
                picture = Resources.Load<Sprite>(prefix + "/sprites/" + locale.Key),
                rarity = r,
                pack = prefix
            });
        }
        return result;
    }

    public static Dictionary<string, string> LoadFile(string prefix, Languages lang)
    {
        if (lang == Languages.Null)
        {
            return null;
        }
        string fileName = prefix + "_" + lang.ToString().ToLower();
        TextAsset asset = Resources.Load(prefix + "/" + fileName) as TextAsset;
        if (asset == null)
        {
            throw new MissingReferenceException("Could not find the " + fileName + " JSON file");
        }
        else
        {
            return JsonUtility.FromJson<LocalizationData>(asset.ToString()).items.ToDictionary(i => i.key, i => i.value);
        }
    }

    public static string GetRandomKey()
    {
        return dico.ElementAt(rand.Next(0, dico.Count)).Key;
    }

    public static List<string> GetRandomKeys(int count)
    {
        if (count > dico.Count)
        {
            return null;
        }
        List<string> output = new List<string>();
        while (output.Count < count)
        {
            string a = GetRandomKey();
            while (output.Contains(a))
            {
                a = GetRandomKey();
            }
            output.Add(a);
        }
        return output;
    }

    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public static Sprite Picture(string key, bool m = false)
    {
        if (m && menu.ContainsKey(key))
        {
            return menu[key].picture;
        }
        else if (dico.ContainsKey(key))
        {
            return dico[key].picture;
        }
        return null;
    }

    public static string Locale(string key, bool m = false)
    {
        if (m && menu.ContainsKey(key))
        {
            return FirstCharToUpper(menu[key].localText);
        }
        else if (dico.ContainsKey(key))
        {
            return FirstCharToUpper(dico[key].localText);
        }
        return null;
    }

    public static string Foreign(string key, bool m = false)
    {
        if (m && menu.ContainsKey(key))
        {
            return FirstCharToUpper(menu[key].foreignText);
        }
        else if (dico.ContainsKey(key))
        {
            return FirstCharToUpper(dico[key].foreignText);
        }
        return null;
    }

    public static AudioClip Audio(string key, bool m = false)
    {
        if (m && menu.ContainsKey(key))
        {
            return menu[key].foreignVoice;
        }
        else if (dico.ContainsKey(key))
        {
            return dico[key].foreignVoice;
        }
        return null;
    }

    public static string FirstCharToUpper(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        return char.ToUpper(s[0]) + s.Substring(1).ToLower();
    }

    public static DicoPackage GetPackage(string key)
    {
        DicoPackage result = new DicoPackage();
        if (packs.TryGetValue(key, out result) == false)
        {
            DicoItem i = null;
            if (dico.TryGetValue(key, out i))
            {
                packs.TryGetValue(i.pack, out result);
            }
        }
        return result;
    }
}
