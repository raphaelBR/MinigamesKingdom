using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public List<CustomAnimation> tabsAnim;
    public List<CustomAnimation> lobbyAnim;
    public List<CustomAnimation> settingsAnim;
    public CustomAnimation lowerBarScrub;

    LanguageSettings langSettings;
    bool langSet;
    int currentMenu;

    private void Awake()
    {
        langSettings = FindObjectOfType<LanguageSettings>();
        currentMenu = 0;
        tabsAnim[currentMenu].Teleport(1);
        if (Parameters.Locale == Languages.Null || Parameters.Foreign == Languages.Null)
        {
            SetLang();
        }
        else
        {
            langSet = true;
        }
    }

    public void SetLang()
    {
        langSettings.Setup();
        SetSettings(2);
        langSet = false;
    }

    public void LangChanged()
    {
        if (langSet)
        {
            SetMenu(3);
        }
        else
        {
            SetMenu(0);
        }
        langSet = true;
    }

    public void SetMenu(int i)
    {
        if (i == currentMenu)
        {
            return;
        }
        lowerBarScrub.Play(currentMenu, i);
        if (i > currentMenu)
        {
            tabsAnim[currentMenu].Play(1, 0);
            tabsAnim[i].Play(2, 1);
        }
        else
        {
            tabsAnim[currentMenu].Play(1, 2);
            tabsAnim[i].Play(0, 1);
        }
        currentMenu = i;
    }

    public void SetSettings(int i)
    {
        SetMenu(3);
    }

    public void SetLobby(int i)
    {
        SetMenu(0);
    }
}
