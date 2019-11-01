using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AssembleItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Text t;
    public RectTransform rect;
    public CustomAnimRect anim;

    [HideInInspector]
    public AssembleSpot spot;
    [HideInInspector]
    public string c;
    AssembleSpawner master;
    [HideInInspector]
    public RectTransform root;

    public void Init(string ch, AssembleSpawner m)
    {
        c = ch;
        master = m;
        t.text = c.ToString().ToUpper();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        rect.SetAsLastSibling();
        master.selected = this;
        if (spot != null)
        {
            spot.img.enabled = true;
            spot.docked = null;
            spot = null;
        }
        //foreach (var box in errors)
        //{
        //    box.error.GoTo(1);
        //}
        //foreach (var box in master.boxes)
        //{
        //    box.blocked = errors.Contains(box);
        //}
        //anim.SetBool("Drag", true);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = master.mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f));
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(DragEnd());
        //foreach (var box in errors)
        //{
        //    box.error.GoTo(0);
        //}
        //anim.SetBool("Drag", false);
    }

    IEnumerator DragEnd()
    {
        yield return null;
        yield return null;
        yield return null;
        master.selected = null;
        if (spot == null)
        {
            anim.state[1].status = root;
        }
        StartCoroutine(Replace());
    }

    public IEnumerator Replace()
    {
        var start = rect.position;
        for (float f = 0f; f < 1f; f += Time.deltaTime / anim.state[1].duration)
        {
            rect.position = Vector2.Lerp(start, anim.state[1].status.position, anim.state[1].anim.Evaluate(f));
            yield return null;
        }
        rect.position = anim.state[1].status.position;
        //rect.anchorMin = anim.state[1].status.anchorMin;
        //rect.anchorMax = anim.state[1].status.anchorMax;
        //rect.offsetMin = anim.state[1].status.offsetMin;
        //rect.offsetMax = anim.state[1].status.offsetMax;
        //rect.position = anim.state[1].status.position;
    }
}
