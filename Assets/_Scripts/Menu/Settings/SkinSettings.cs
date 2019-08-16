using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Events;

[System.Serializable]
public class ButtonsBundle
{
    public GameObject parent;
    [HideInInspector]
    public Button[] members;
    [HideInInspector]
    public Outline[] outlines;
    [HideInInspector]
    public UnityEvent onSelect;
    [HideInInspector]
    public Button selected;

    public void Init()
    {
        members = parent.GetComponentsInChildren<Button>();
        outlines = parent.GetComponentsInChildren<Outline>();
        for (int i = 0; i < members.Length; i++)
        {
            int i2 = i;
            members[i].onClick.RemoveAllListeners();
            members[i].onClick.AddListener(delegate { Select(i2); });
            //outlines[i].enabled = false;
        }
    }

    public void Select(int i, bool call = true)
    {
        selected = members[i];
        //outlines[i].enabled = true;

        for (int j = 0; j < members.Length; j++)
        {
            members[j].interactable = (i != j);
            if (i != j)
            {
                outlines[j].effectColor = Color.black;
            }
            else
            {
                outlines[j].effectColor = Color.white;
            }
        }
        if (call)
        {
            onSelect.Invoke();
        }
    }
}

/// <summary>
/// The interface used to customize the Game’s visuals.
/// </summary>
[RequireComponent(typeof(ScrollRect))]
public class SkinSettings : MonoBehaviour {

    public GameObject noCustom;
    public SkinPreview prefab;
    public CustomAnimation customization;
    public ButtonsBundle contents;
    public ButtonsBundle backs;
    public ButtonsBundle clickables;
    public ButtonsBundle highlights;
    public ButtonsBundle goods;
    public ButtonsBundle bads;
    public List<CustomColor> refreshCustomize = new List<CustomColor>();

    Transform content;
    List<Skin> allSkins = new List<Skin>();
    List<SkinPreview> allPreviews = new List<SkinPreview>();
    Skin custom;
    Skin backup;
    SkinPreview customPreview;
    string CustomPath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, "customSkin.json");
        }
    }

    void Start()
    {
        content = GetComponent<ScrollRect>().content;
        var temp = Resources.LoadAll<TextAsset>("Skins/");
        foreach (var item in temp)
        {
            allSkins.Add(JsonUtility.FromJson<Skin>(item.text));
        }
        if (Parameters.Skin == null)
        {
            Parameters.Skin = allSkins[0];
        }
        foreach (var skin in allSkins)
        {
            SkinPreview s = Instantiate(prefab, content);
            allPreviews.Add(s);
            s.Init(skin, this);
        }
        customPreview = Instantiate(prefab, content);
        customPreview.transform.SetAsFirstSibling();
        noCustom.transform.SetAsLastSibling();

        contents.Init();
        contents.onSelect.AddListener(delegate { Setup(contents.selected, ColorType.Content); });
        backs.Init();
        backs.onSelect.AddListener(delegate { Setup(backs.selected, ColorType.Back); });
        goods.Init();
        goods.onSelect.AddListener(delegate { Setup(goods.selected, ColorType.Good); });
        bads.Init();
        bads.onSelect.AddListener(delegate { Setup(bads.selected, ColorType.Bad); });
        clickables.Init();
        clickables.onSelect.AddListener(delegate { Setup(clickables.selected, ColorType.Clickable); });
        highlights.Init();
        highlights.onSelect.AddListener(delegate { Setup(highlights.selected, ColorType.Highlight); });

        Init();
        Reset();
    }

    public void Reset()
    {
        customization.Teleport(0);
    }

    public void Init()
    {
        if (File.Exists(CustomPath))
        {
            using (StreamReader streamReader = File.OpenText(CustomPath))
            {
                string jsonString = streamReader.ReadToEnd();
                custom = JsonUtility.FromJson<Skin>(jsonString);
            }
            noCustom.SetActive(false);
            customPreview.gameObject.SetActive(true);
            customPreview.Init(custom, this, true);
        }
        else
        {
            noCustom.SetActive(true);
            customPreview.gameObject.SetActive(false);
        }
    }

    public void Validate(Skin s)
    {
        if (s != null)
        {
            Parameters.Skin = s;
            var customColors = FindObjectsOfType<CustomColor>();
            foreach (CustomColor c in customColors)
            {
                c.RefreshColor();
            }
        }
        foreach (SkinPreview sk in allPreviews)
        {
            sk.RefreshInteractable();
        }
        if (custom != null)
        {
            customPreview.RefreshInteractable();
        }
    }

    public void StartCustomize()
    {
        backup = custom;
        if (custom == null)
        {
            custom = Parameters.Skin;
        }
        foreach (CustomColor cust in refreshCustomize)
        {
            cust.SetSkin(custom);
        }
        
        foreach (Button b in contents.members)
        {
            b.interactable = (b.image.color != custom.content);
        }
        foreach (Button b in clickables.members)
        {
            b.interactable = (b.image.color != custom.clickable);
        }
        foreach (Button b in backs.members)
        {
            b.interactable = (b.image.color != custom.back);
        }
        foreach (Button b in goods.members)
        {
            b.interactable = (b.image.color != custom.good);
        }
        foreach (Button b in bads.members)
        {
            b.interactable = (b.image.color != custom.bad);
        }
        foreach (Button b in highlights.members)
        {
            b.interactable = (b.image.color != custom.highlight);
        }

        customization.Play(0, 1);
    }

    public void EndCustomize(bool keepChanges)
    {
        if (keepChanges)
        {
            custom.theme = "My Skin";
            Validate(custom);

            string jsonString = JsonUtility.ToJson(custom);
            using (StreamWriter streamWriter = File.CreateText(CustomPath))
            {
                streamWriter.Write(jsonString);
            }
        }
        else
        {
            custom = backup;
            foreach (CustomColor cust in refreshCustomize)
            {
                cust.RefreshColor();
            }
        }
        Init();
        customization.Play(1, 0);
    }

    public void Setup(Button bu, ColorType type)
    {
        Color color = bu.image.color;
        switch (type)
        {
            case ColorType.Content:
                custom.content = color;
                break;
            case ColorType.Clickable:
                custom.clickable = color;
                break;
            case ColorType.Back:
                custom.back = color;
                break;
            case ColorType.Bad:
                custom.bad = color;
                break;
            case ColorType.Good:
                custom.good = color;
                break;
            case ColorType.Highlight:
                custom.highlight = color;
                break;
            default:
                break;
        }
        foreach (CustomColor cust in refreshCustomize)
        {
            cust.SetSkin(custom);
        }
    }
}
