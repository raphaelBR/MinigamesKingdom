using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// The interface used to change to game’s font.
/// </summary>
[RequireComponent(typeof(ScrollRect))]
public class FontSettings : MonoBehaviour {

    public Button prefab;
    public TextFont[] localTexts;

    Transform content;
    List<Font> allFonts;
    List<Button> allButtons = new List<Button>();
    TextFont[] textFonts;

    void Start () {
        content = GetComponent<ScrollRect>().content;
        allFonts = Resources.LoadAll<Font>("Fonts/").ToList();
        allFonts.Add(Resources.GetBuiltinResource<Font>("Arial.ttf"));
        if (Parameters.Font == null)
        {
            Parameters.Font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }
        foreach (var font in allFonts)
        {
            SpawnButton(font);
        }
	}

    public void Reset()
    {
        foreach (var button in allButtons)
        {
            button.gameObject.SetActive(Account.IsUnlocked(UnlockType.Font, button.name));
        }
    }

    void SpawnButton(Font f) {
        Button b = Instantiate(prefab, content);
        Text t = b.GetComponentInChildren<Text>();
        b.name = f.fontNames[0];
        t.font = f;
        t.text = f.fontNames[0];
        b.onClick.AddListener(delegate { Validate(f); });
        allButtons.Add(b);
        b.interactable = (f != Parameters.Font);
    }

    void Validate(Font f)
    {
        if (f != null)
        {
            Parameters.Font = f;
            foreach (TextFont t in localTexts)
            {
                t.RefreshFont();
            }
        }
        foreach (Button bu in allButtons)
        {
            bu.interactable = (bu.GetComponentInChildren<Text>().font != Parameters.Font);
        }
    }

    public void Apply()
    {
        textFonts = FindObjectsOfType<TextFont>();
        foreach (var t in textFonts)
        {
            t.RefreshFont();
        }
    }
}
