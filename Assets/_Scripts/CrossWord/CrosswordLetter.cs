using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class CrosswordLetter : CrosswordTile
{
    
    public Text t;
    [HideInInspector]
    public bool valid;
    [HideInInspector]
    public string solution;

    public void Init(string s)
    {
        valid = false;
        selected.SetActive(false);
        t.text = "";
        solution = s;
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

    public bool CompareSolution()
    {
        if (!valid)
        {
            if (string.Compare(solution, t.text, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace) == 0)
            {
                valid = true;
                back.color = validColor;
                master.Success();
            }
        }
        return valid;
    }
}
