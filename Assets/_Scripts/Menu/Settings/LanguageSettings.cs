using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.Events;

/// <summary>
/// The interface used to change the game’s languages.
/// </summary>
public class LanguageSettings : MonoBehaviour {

    public TextLocalized languageHeader;
    public Text localeHeader;
    public Button[] locals;
    [Space(5f)]
    public CustomAnimation foreign;
    public Text foreignHeader;
    public Button[] foreigns;
    [Space(5f)]
    public CustomAnimation validate;
    public UnityEvent onValidate;
    public CustomAnimation lowerBar;

    bool flag1;
    bool flag2;
    List<Languages> langList = new List<Languages>();
    TextLocalized[] localized;

    public void Setup()
    {
        localized = FindObjectsOfType<TextLocalized>();
        langList = Enum.GetValues(typeof(Languages)).Cast<Languages>().ToList();
        if (Parameters.Locale != Languages.Null)
        {
            foreign.Play(0, 1);
        }
        else
        {
            foreign.Teleport(0);
        }
        if (Parameters.Foreign != Languages.Null)
        {
            validate.Play(0, 1);
        }
        else
        {
            validate.Teleport(0);
        }
        if (Parameters.Locale == Languages.Null || Parameters.Foreign == Languages.Null)
        {
            lowerBar.GoTo(0);
        }
        if (Parameters.Locale != Languages.Null)
        {
            PrepareForeigns();
        }
        RefreshTexts();
    }

    void PrepareForeigns()
    {
        int i = 0;
        foreach (Languages l in langList)
        {
            if (l != Languages.Null && l != Parameters.Locale)
            {
                var button = foreigns[i];
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(delegate { SetForeign(l); });
                button.image.sprite = Resources.Load<Sprite>("Flags/" + l.ToString());
                button.name = l.ToString();
                i++;
            }
        }
    }

    public void SetLocale(string s)
    {
        Parameters.Locale = (Languages)Enum.Parse(typeof(Languages), s, true);
        Parameters.Foreign = Languages.Null;
        PrepareForeigns();
        foreign.Play(0, 1);
        validate.GoTo(0);
        lowerBar.GoTo(0);
        Dico.LoadMenus();
        RefreshTexts();
    }

    public void SetForeign(Languages lang)
    {
        Parameters.Foreign = lang;
        validate.Play(0, 1);
        RefreshTexts();
        Progress.Load();
        Dico.LoadMenus();
        foreach (TextLocalized t in localized)
        {
            t.RefreshText();
        }
    }

    public void RefreshTexts()
    {
        // Replace with Dico entries
        foreach (Button bu in locals)
        {
            bu.interactable = (bu.name != Parameters.Locale.ToString());
        }
        foreach (Button bu in foreigns)
        {
            bu.interactable = (bu.name != Parameters.Foreign.ToString());
        }
        if (Parameters.Locale == Languages.Null)
        {
            localeHeader.text = Dico.Locale("i_speak", true) + " ...";
        }
        else
        {
            localeHeader.text = Dico.Locale("i_speak", true) + " " + Dico.Locale(Parameters.Locale.ToString().ToLower(), true);
        }
        if (Parameters.Foreign == Languages.Null)
        {
            foreignHeader.text = Dico.Locale("i_want_to_learn", true) + " ...";
        }
        else
        {
            foreignHeader.text = Dico.Locale("i_want_to_learn", true) + " " + Dico.Locale(Parameters.Foreign.ToString().ToLower(), true);
        }
        languageHeader.RefreshText();
    }

    public void Validate()
    {
        onValidate.Invoke();
    }

}
