using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform), typeof(Animator))]
public class BasketItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image img;
    public Text locale;
    public float replaceDuration = 0.5f;
    public AnimationCurve replaceAnim;

    RectTransform rect;
    [HideInInspector]
    public Basket master;
    [HideInInspector]
    public string k;
    [HideInInspector]
    public List<BasketBox> errors = new List<BasketBox>();
    Animator anim;

    public void Init(string key)
    {
        rect = GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
        k = key;
        img.sprite = Dico.Picture(key);
        locale.text = Dico.Locale(key).ToUpper();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        rect.SetAsLastSibling();
        master.selected = this;
        foreach (var box in errors)
        {
            box.error.GoTo(1);
        }
        foreach (var box in master.boxes)
        {
            box.blocked = errors.Contains(box);
        }
        anim.SetBool("Drag", true);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 100f));
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(Replace());
        foreach (var box in errors)
        {
            box.error.GoTo(0);
        }
        anim.SetBool("Drag", false);
    }

    public IEnumerator Replace()
    {
        var start = rect.anchoredPosition;
        for (float f = 0f; f < 1f; f+= Time.deltaTime / replaceDuration)
        {
            rect.anchoredPosition = Vector2.Lerp(start, Vector2.zero, replaceAnim.Evaluate(f));
            yield return null;
        }
        rect.anchoredPosition = Vector2.zero;
    }
}
