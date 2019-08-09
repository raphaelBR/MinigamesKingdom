using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class BasketBox : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public Text question;
    public Transform answerPrepare;
    public Transform answerPlace;
    public float placeDuration = 0.5f;
    public AnimationCurve placeAnim;
    public ParticleSystem win;
    public GameObject front;

    [HideInInspector]
    public Basket master;
    [HideInInspector]
    public string k;
    [HideInInspector]
    public Animator anim;

    public void Init(string key)
    {
        anim = GetComponent<Animator>();
        k = key;
        question.text = Dico.Foreign(key).ToUpper();
        front.SetActive(false);
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        StartCoroutine(master.Fuse(this));
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        anim.SetBool("Hover", false);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            anim.SetBool("Hover", true);
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("Hover", false);
    }

    public IEnumerator CloseBox(BasketItem item)
    {
        anim.SetTrigger("Win");
        var rect = item.transform as RectTransform;
        var pos = rect.position;
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);

        rect.parent = answerPrepare;
        rect.position = pos;
        yield return StartCoroutine(item.Replace());
        Destroy(item);
        front.SetActive(true);
        
        for (float f = 0f; f < 1f; f += Time.deltaTime / placeDuration)
        {
            rect.position = Vector3.Lerp(answerPrepare.position, answerPlace.position, placeAnim.Evaluate(f));
            yield return null;
        }
        anim.SetTrigger("Success");
        win.Play();
        Destroy(this);
    }

}
