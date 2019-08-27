using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The interface used to navigate between menus.
/// </summary>
public class MainMenu : MonoBehaviour {
    
    public List<CustomAnimRect> tabsAnim;
    public List<Button> tabsButton;
    public CustomAnimRect lowerBarScrub;
    public CustomAnimRect lowerBar;

    LanguageSettings langSettings;
    bool langSet;
    int currentMenu;

    private void Awake()
    { 
        currentMenu = 0;
        for (int i = 0; i < tabsButton.Count; i++)
        {
            tabsButton[i].interactable = (i != currentMenu);
        }
        tabsAnim[currentMenu].Teleport(1);
        if (Parameters.Locale == Languages.Null || Parameters.Foreign == Languages.Null)
        {
            SetLang();
        }
        else
        {
            langSet = true;
            lowerBar.GoTo(1);
        }
    }

    public void SetLang()
    {
        FindObjectOfType<LanguageSettings>().Setup();
        SetMenu(3);
        FindObjectOfType<Tweaks>().SelectSubTab(2);
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
        lowerBar.GoTo(1);
        Dico.LoadPackages();
        Dico.LoadMenus();
    }

    public void SetMenu(int i)
    {
        if (i == currentMenu)
        {
            return;
        }
        tabsButton[currentMenu].interactable = true;
        tabsButton[i].interactable = false;
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
}
