using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AssembleSpot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public RectTransform rect;
    public CustomAnimImage hover;
    public Image img;

    [HideInInspector]
    public bool blocked;
    [HideInInspector]
    public AssembleSpawner master;
    [HideInInspector]
    public Assemble group;
    [HideInInspector]
    public AssembleItem docked;
    bool locked;

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (!locked && master.selected != null)
        {
            docked = master.selected;
            docked.spot = this;
            docked.anim.state[1].status = rect;
            docked.StartCoroutine(docked.Replace());
            img.enabled = false;
            group.Verify();
        }
    }

    public void Eject()
    {
        docked.anim.state[1].status = docked.root;
        docked.StartCoroutine(docked.Replace());
        docked.spot = null;
        docked = null;
        img.enabled = true;
    }

    public void Lock()
    {
        if (locked == false)
        {
            master.Progress();
            img.enabled = true;
            locked = true;
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        hover.GoTo(0);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (!locked && Input.GetMouseButton(0) && master.selected != null)
        {
            hover.GoTo(1);
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        hover.GoTo(0);
    }

}
