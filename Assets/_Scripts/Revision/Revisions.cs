using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Revisions : MonoBehaviour {

    public RevisionItem item;
    public float spacing = 100f;

    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(0f, spacing * Dico.dico.Count);
        float f = 0f;
        foreach (KeyValuePair<string, Item> entry in Dico.dico)
        {
            RevisionItem i = Instantiate(item, rect);
            i.transform.localPosition = new Vector3(0f, -f * spacing, 0f);
            i.Init(entry.Value);
            f++;
        }
    }
}
