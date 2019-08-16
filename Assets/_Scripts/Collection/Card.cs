using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Holds the data about a learning subject.
/// </summary>
public class Card : MonoBehaviour {

    [Header("Function")]
    public Toggle favorite;
    public GameObject dummy;
    public CustomAnimation[] details;
    [Header("Infos")]
    public Image cardTemplate;
    public Image picture;
    public Text locale;
    public Text foreign;
    public SoundPlayer sound;
    [Header("XP Bar")]
    public Image outline;
    public Image nextBar;
    public Image expFill;
    public Sprite[] levelsBar;
    [Header("XP Percent")]
    public Text expPercent;
    public Sprite[] levelsBall;
    public Image ball;

    CustomAnimation anim;
    
    [HideInInspector]
    public string key;
    Collection col;

    CardPack pack;
    [HideInInspector]
    public int siblingIndex;
    Transform parent;
    Coroutine zoom;
    Coroutine zoom2;
    [HideInInspector]
    public GameObject activeDummy;
    bool firstFlag;
    bool on = false;
    Sprite spr;

    public void Reset()
    {
        on = false;
        firstFlag = false;
        anim.Teleport(0);
        foreach (var detail in details)
        {
            detail.Teleport(0);
        }
        transform.SetParent(parent);
        transform.SetSiblingIndex(siblingIndex);
        if (activeDummy != null)
        {
            Destroy(activeDummy);
            activeDummy = null;
        }
    }

    public void Init(string k, CardPack p)
    {
        anim = GetComponent<CustomAnimation>();
        col = FindObjectOfType<Collection>();
        anim.state[0].status = p.bigSpot;
        parent = transform.parent;
        pack = p;
        firstFlag = false;

        key = k;
        cardTemplate.sprite = Dico.GetPackage(key).cardBack;
        picture.sprite = Dico.Picture(key);
        spr = picture.sprite;
        locale.text = Dico.Locale(key);
        foreign.text = Dico.Foreign(key);
        sound.sound.clip = Dico.Audio(key);
        favorite.SilentSet(Progress.IsFavorite(key));
        if (Progress.GetExp(key).Level >= Progress.cap.userCap.Length)
        {
            expPercent.text = "★";
            expFill.fillAmount = 0f;
        }
        else
        {
            var percent = (float)Progress.GetExp(key).Xp / Progress.GetCap(key);
            expPercent.text = (Mathf.FloorToInt(percent * 100f).ToString().PadLeft(2, '0')) + "%";
            expFill.fillAmount = percent;
        }

        int i = Progress.GetExp(key).Level;
        outline.sprite = levelsBar[i - 1];
        ball.sprite = levelsBall[i - 1];
        if (i < levelsBar.Length)
        {
            nextBar.sprite = levelsBar[i];
        }
    }

    public void ToggleFavorite()
    {
        bool b = !Progress.IsFavorite(key);
        Progress.SetFavorite(key, b);
        if (col != null && b == false && !on)
        {
            col.Refresh();
        }
    }

    public void ToggleDisplay()
    {
        on = !on;
        int o = 0;
        if (on)
        {
            o = 1;
        }
        foreach (var detail in details)
        {
            detail.GoTo(o);
        }
        if (on)
        {
            pack.SetCardActive(this);
            if (col != null)
            {
                col.SetAdvancedFilters(false);
            }
        }
        else
        {
            pack.SetCardActive(null);
        }
        if (zoom != null)
        {
            StopCoroutine(zoom);
        }
        if (zoom2 != null)
        {
            StopCoroutine(zoom2);
        }
        zoom = StartCoroutine(Travel(on));
    }

    public IEnumerator Travel(bool zoomingIn)
    {
        if (zoomingIn)
        {
            if (firstFlag == false)
            {
                siblingIndex = transform.GetSiblingIndex();
                firstFlag = true;
            }
            if (activeDummy == null)
            {
                activeDummy = Instantiate(dummy, parent);
                activeDummy.transform.SetSiblingIndex(siblingIndex);
                anim.state[1].status = activeDummy.GetComponent<RectTransform>();
            }
            transform.SetParent(pack.transform, true);
        }
        
        var c = StartCoroutine(SpriteHack());
        if (zoomingIn)
        {
            zoom2 = StartCoroutine(anim.Transition(1, 0));
        }
        else
        {
            zoom2 = StartCoroutine(anim.Transition(0, 1));
        }
        yield return zoom2;
        if (!zoomingIn)
        {
            Destroy(activeDummy);
            activeDummy = null;
            transform.SetParent(parent);
            transform.SetSiblingIndex(siblingIndex);
            firstFlag = false;
        }
        StopCoroutine(c);
    }

    IEnumerator SpriteHack()
    {
        while (true)
        {
            picture.sprite = null;
            picture.sprite = spr;
            yield return null;
        }
    }
}
