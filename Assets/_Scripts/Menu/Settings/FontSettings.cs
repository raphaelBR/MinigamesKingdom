using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(ScrollRect))]
public class FontSettings : MonoBehaviour {

    public Button prefab;
    public UnityEvent onValidate;

    Transform content;
    List<Font> allFonts;
    List<Button> allButtons = new List<Button>();
    TextFont[] textFonts;

    void Start () {
        content = GetComponent<ScrollRect>().content;
        textFonts = FindObjectsOfType<TextFont>();
        allFonts = Resources.LoadAll<Font>("Fonts/").ToList();
        allFonts.Add(Resources.GetBuiltinResource<Font>("Arial.ttf"));
        if (Parameters.Font == null)
        {
            Parameters.Font = allFonts[0];
        }
        for (int i = 0; i < allFonts.Count; i++)
        {
            SpawnButton(allFonts[i], i);
        }
	}
	
	void SpawnButton(Font f, int i) {
        Button b = Instantiate(prefab, content);
        Text t = b.GetComponentInChildren<Text>();
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
            textFonts = FindObjectsOfType<TextFont>();
            foreach (TextFont t in textFonts)
            {
                t.RefreshFont();
            }
        }
        foreach (Button bu in allButtons)
        {
            bu.interactable = (bu.GetComponentInChildren<Text>().font != Parameters.Font);
        }
        onValidate.Invoke();
    }

    public void Cancel()
    {
        onValidate.Invoke();
    }
}
