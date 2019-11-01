using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BasketBox : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public Text question;
    public Transform answerPrepare;
    public Transform answerPlace;
    public float placeDuration = 0.5f;
    public AnimationCurve placeAnim;
    public ParticleSystem win;
    public GameObject front;
    public CustomAnimImage hover;
    public CustomAnimRect success;
    public CustomAnimRect error;
    public CustomAnimImage mistake;
    public CustomAnimRect errorShake;
    public CustomAnimRect finished;
    public bool blocked;

    [HideInInspector]
    public Basket master;
    [HideInInspector]
    public string k;

    public void Init(string key)
    {
        blocked = false;
        k = key;
        question.text = Dico.Foreign(key).ToUpper();
        front.SetActive(false);
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (!blocked)
        {
            StartCoroutine(master.Fuse(this));
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        hover.GoTo(0);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0) && !blocked)
        {
            hover.GoTo(1);
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        hover.GoTo(0);
    }

    public IEnumerator Success(BasketItem item)
    {
        var rect = item.transform as RectTransform;
        var pos = rect.position;
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        foreach (var it in master.items)
        {
            it.errors.Remove(this);
        }
        error.Teleport(0);
        error.enabled = false;

        rect.parent = answerPrepare;
        rect.position = pos;
        hover.GoTo(0);
        success.GoTo(1);
        yield return StartCoroutine(item.Replace());
        Destroy(item);
        front.SetActive(true);

        for (float f = 0f; f < 1f; f += Time.deltaTime / placeDuration)
        {
            rect.position = Vector3.Lerp(answerPrepare.position, answerPlace.position, placeAnim.Evaluate(f));
            yield return null;
        }
        finished.GoTo(1);
        win.Play();
        Destroy(this);
    }

    public void Fail()
    {
        hover.GoTo(0);
        mistake.PlayChain();
        errorShake.PlayChain();
    }
}
