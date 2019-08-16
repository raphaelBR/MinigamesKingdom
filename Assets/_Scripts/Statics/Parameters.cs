using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Holds the player customization preferences.
/// </summary>
public static class Parameters
{
    static Settings settings = new Settings();
    static string Datapath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, "settings.json");
        }
    }
    public static Font Font
    {
        get
        {
            if (settings.currentFont == null)
            {
                settings.currentFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
                Save();
            }
            return settings.currentFont;
        }
        set
        {
            settings.currentFont = value;
            Save();
        }
    }
    public static Languages Locale
    {
        get
        {
            return settings.locale;
        }
        set
        {
            settings.locale = value;
            Save();
        }
    }
    public static Languages Foreign
    {
        get
        {
            return settings.foreign;
        }
        set
        {
            settings.foreign = value;
            Save();
        }
    }
    public static Skin Skin
    {
        get
        {
            return settings.currentSkin;
        }
        set
        {
            settings.currentSkin = value;
            Save();
        }
    }

    static Parameters()
    {
        if (File.Exists(Datapath))
        {
            Load();
        }
        else
        {
            Save();
        }
    }

    public static void Save()
    {
        string jsonString = JsonUtility.ToJson(settings);

        using (StreamWriter streamWriter = File.CreateText(Datapath))
        {
            streamWriter.Write(jsonString);
        }
    }

    public static void Load()
    {
        using (StreamReader streamReader = File.OpenText(Datapath))
        {
            string jsonString = streamReader.ReadToEnd();
            settings = JsonUtility.FromJson<Settings>(jsonString);
        }
    }

    public static void Wipe()
    {
        File.Delete(Datapath);
        settings = new Settings();
    }
}
