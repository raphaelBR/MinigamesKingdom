using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosswordTile : MonoBehaviour
{
    public GameObject blankPrefab;
    public CustomAnimRect anim;
    public Image back;
    public Color validColor;
    [HideInInspector]
    public GameObject selected;
    //public List<RectTransform> selectedSlots = new List<RectTransform>();
    
    public RectTransform r;
    [HideInInspector]
    public Vector2 pos;
    protected CrosswordsAlgorithm master;

    public virtual void Init(Vector2 s, Vector2 p, CrosswordsAlgorithm m)
    {
        master = m;
        pos = p;

        var dummy = Instantiate(blankPrefab, transform.parent).GetComponent<RectTransform>();
        dummy.name = name + "_root";
        dummy.anchorMin = r.anchorMin;
        dummy.anchorMax = r.anchorMax;
        dummy.offsetMin = r.offsetMin;
        dummy.offsetMax = r.offsetMax;
        dummy.pivot = r.pivot;
        dummy.rotation = r.rotation;
        dummy.localScale = r.localScale;
        dummy.position = r.position;

        dummy.sizeDelta = s;
        dummy.anchoredPosition = pos * s;

        anim.state[0].status = dummy;
        anim.Teleport(0);

    }

}
