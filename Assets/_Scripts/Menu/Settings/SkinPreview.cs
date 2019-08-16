using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The interface used to create a custom palette.
/// </summary>
public class SkinPreview : MonoBehaviour
{
    public Text theme;
    public Image[] content;
    public Image good;
    public Image bad;
    public Image back;
    public Image highlight;
    public Image clickable;
    public Button button;
    public Button edit;

    public void Init(Skin s, SkinSettings set, bool custom = false)
    {
        if (custom)
        {
            edit.onClick.RemoveAllListeners();
            edit.onClick.AddListener(delegate { set.StartCustomize(); });
        }
        edit.gameObject.SetActive(custom);
        theme.text = s.theme;
        theme.color = s.content;
        foreach (Image img in content)
        {
            img.color = s.content;
        }
        good.color = s.good;
        bad.color = s.bad;
        back.color = s.back;
        highlight.color = s.highlight;
        clickable.color = s.clickable;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { set.Validate(s); });
        RefreshInteractable();
    }

    public void RefreshInteractable()
    {
        button.interactable = (theme.text != Parameters.Skin.theme);
    }
}