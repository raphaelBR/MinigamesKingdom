using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EntryType
{
    Empty,
    Hint,
    Valid
}

public class WordscapePreviewLetter : MonoBehaviour
{
    public Text text;
    public CustomAnimRect inputAnim;
    public Color emptyColor;
    public Color hintColor;
    public Color validColor;

    public void SetText(string t, EntryType e)
    {
        if (t == null)
        {
            t = "?";
        }
        else
        {
            t = t.ToUpper();
        }
        switch (e)
        {
            case EntryType.Empty:
                inputAnim.GoTo(0);
                text.color = emptyColor;
                break;
            case EntryType.Hint:
                inputAnim.GoTo(1);
                text.color = hintColor;
                break;
            case EntryType.Valid:
                inputAnim.GoTo(2);
                text.color = validColor;
                break;
            default:
                break;
        }
        text.text = t;
    }
}
