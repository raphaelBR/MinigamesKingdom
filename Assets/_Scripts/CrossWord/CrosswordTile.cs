using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosswordTile : MonoBehaviour
{
    public Color validColor;
    public Image back;
    public GameObject selected;
    public InputField field;
    public Button btn;
    public RectTransform rect;
    public Text t;
    public Image i;
    [HideInInspector]
    public Vector2 pos;
    public List<CrosswordTile> tiles = new List<CrosswordTile>();
    string lastField;
    [HideInInspector]
    public string solution;
    bool valid;

    public void Init(Vector2 s, Vector2 p)
    {
        valid = false;
        selected.SetActive(false);
        t.text = "";
        lastField = "";
        i.enabled = false;
        btn.enabled = false;
        pos = p;
        rect.sizeDelta = s;
        rect.anchoredPosition = pos * s;
    }

    public void SetText(string s)
    {
        if (valid == false)
        {
            t.text = s.ToUpper();
        }
    }

    public void SetText(char c)
    {
        if (valid == false)
        {
            t.text = c.ToString().ToUpper();
        }
    }

    public void SetClue(string key)
    {
        i.enabled = true;
        btn.enabled = true;
        i.sprite = Dico.Picture(key);
    }

    public void ToggleSet()
    {
        field.Select();
        SetActive(Mathf.Min(field.text.Length, tiles.Count - 1));
        field.text = "";
    }

    public void SetActive(int j)
    {
        if (j < tiles.Count && j >= 0)
            tiles[j].transform.SetAsLastSibling();
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].selected.SetActive(i == j);
        }
    }

    public void Refresh()
    {
        if (field.text.Length > tiles.Count)
        {
            field.text = field.text.Substring(0, Mathf.Max(field.text.Length - 2, 1)) + field.text.Substring(Mathf.Max(field.text.Length - 1, 0), 1);
        }
        else
        {
            SetActive(Mathf.Min(field.text.Length, tiles.Count - 1));
        }

        for (int i = 0; i <= lastField.Length; i++)
        {
            if (i < field.text.Length)
            {
                tiles[i].SetText(field.text[i]);
            }
            else if (i < tiles.Count)
            {
                tiles[i].SetText("");
            }
        }
        lastField = field.text;
    }

    public void CompareSolution()
    {
        // Comparison need to handle accents
        if (solution == t.text)
        {
            back.color = validColor;
            valid = true;
        }
    }

    public void Validate()
    {
        foreach (var tile in tiles)
        {
            tile.CompareSolution();
        }
        SetActive(-1);
    }

}
