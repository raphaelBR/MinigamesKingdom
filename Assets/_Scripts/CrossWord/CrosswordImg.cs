using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosswordImg : CrosswordTile
{
    public InputField field;
    public Image i;
    public Button btn;
    [HideInInspector]
    public List<CrosswordLetter> tiles = new List<CrosswordLetter>();
    string word;

    public void Init(string key, RectTransform spot)
    {
        word = key;
        field.characterLimit = Dico.Foreign(key).Length;
        i.sprite = Dico.Picture(key);
        anim.state[1].status = spot;
    }

    public void SetActive(int j)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].selected.SetActive(i == j);
        }
    }

    public void Validate()
    {
        bool completeTest = true;
        bool typoTest = true;
        foreach (var tile in tiles)
        {
            completeTest = (tile.t.text != "") && completeTest;
        }
        if (completeTest)
        {
            foreach (var tile in tiles)
            {
                typoTest = tile.CompareSolution() && typoTest;
            }
            master.CrossCompletionTest();
            if (typoTest)
            {
                btn.interactable = false;
                back.color = validColor;
                master.WordFull(word);
            }
            else
            {
                master.Damage();
            }
            StartCoroutine(WaitAndClear());
        }
        else
        {
            master.DisableEntry();
            SetActive(-1);
        }
    }

    IEnumerator WaitAndClear()
    {
        yield return new WaitForSeconds(1.5f);
        master.DisableEntry();
        SetActive(-1);
    }

    public void Refresh()
    {
        SetActive(field.text.Length);
        //string s = "";

        for (int i = 0; i < tiles.Count; i++)
        {
            if (i < field.text.Length)
            {
                tiles[i].SetText(field.text[i]);
                //s += tiles[i].t.text;
            }
            else
            {
                tiles[i].SetText("");
            }
        }
        //field.text = s;
    }

    public void ToggleSet()
    {
        master.EnableEntry(this, word);

        field.Select();
        SetActive(Mathf.Min(field.text.Length, tiles.Count - 1));
        field.text = "";
    }
}
