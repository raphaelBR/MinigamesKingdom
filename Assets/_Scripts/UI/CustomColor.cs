using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ColorType
{
    Content,
    Clickable,
    Back,
    Bad,
    Good,
    Highlight
}

/// <summary>
/// Automatically change the visual component’s color to match the player’s customized settings.
/// </summary>
public class CustomColor : MonoBehaviour
{
    public ColorType type;

    Image img;
    Text txt;
    Button button;
    Color c;

    void Start()
    {
        img = GetComponent<Image>();
        txt = GetComponent<Text>();
        button = GetComponent<Button>();
        RefreshColor();
    }

    public void RefreshColor()
    {
        SetSkin(Parameters.Skin);
    }

    public void SetSkin(Skin s)
    {
        switch (type)
        {
            case ColorType.Content:
                c = s.content;
                break;
            case ColorType.Clickable:
                c = s.clickable;
                break;
            case ColorType.Back:
                c = s.back;
                break;
            case ColorType.Bad:
                c = s.bad;
                break;
            case ColorType.Good:
                c = s.good;
                break;
            case ColorType.Highlight:
                c = s.highlight;
                break;
            default:
                break;
        }

        if (txt != null)
        {
            txt.color = c;
        }
        if (button != null)
        {
            button.image.color = Color.white;
            button.colors = new ColorBlock()
            {
                normalColor = c,
                highlightedColor = s.highlight,
                pressedColor = s.clickable,
                selectedColor = s.clickable,
                disabledColor = s.back,
                colorMultiplier = 1f
            };
        }
        else if (img != null)
        {
            img.color = c;
        }
    }
}
