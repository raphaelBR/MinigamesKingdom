using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Basket : MonoBehaviour
{

    public Transform itemParent;
    public BasketItem itemPrefab;
    public Transform boxParent;
    public BasketBox boxPrefab;

    List<BasketItem> items = new List<BasketItem>();
    List<BasketBox> boxes = new List<BasketBox>();
    [HideInInspector]
    public BasketItem selected;

    [HideInInspector]
    public UnityEvent onComplete;

    ScoreBank bank;

    public void Init(List<string> keys, int decoys = 0)
    {
        bank = FindObjectOfType<ScoreBank>();
        for (int i = 0; i < keys.Count; i++)
        {
            var item = Instantiate(itemPrefab, itemParent);
            item.Init(keys[i]);
            item.master = this;
            items.Add(item);
            if (i >= decoys)
            {
                var box = Instantiate(boxPrefab, boxParent);
                box.Init(keys[i]);
                box.master = this;
                boxes.Add(box);
            }
        }
        Dico.Shuffle(items);
        Dico.Shuffle(boxes);
        float ratio = 1f / items.Count;
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i].GetComponent<RectTransform>();
            item.anchorMin = new Vector2(0.5f, ratio * (i + 0.5f));
            item.anchorMax = new Vector2(0.5f, ratio * (i + 0.5f));
            item.anchoredPosition = Vector2.zero;
        }
        ratio = 1f / boxes.Count;
        for (int i = 0; i < boxes.Count; i++)
        {
            var box = boxes[i].GetComponent<RectTransform>();
            box.anchorMin = new Vector2(0f, ratio * i);
            box.anchorMax = new Vector2(1f, ratio * (i + 1));
            box.anchoredPosition = Vector2.zero;
        }
    }

    void Reposition()
    {
        float ratio = 1f / items.Count;
        for (int i = 0; i < items.Count; i++)
        {
            var rect = items[i].GetComponent<RectTransform>();
            var pos = rect.transform.position;
            rect.anchorMin = new Vector2(0.5f, ratio * (i + 0.5f));
            rect.anchorMax = new Vector2(0.5f, ratio * (i + 0.5f));
            rect.position = pos;
            StartCoroutine(items[i].Replace());
        }
    }

    public IEnumerator Fuse(BasketBox box)
    {
        if (selected.k == box.k)
        {
            items.Remove(selected);
            selected.Replace();
            Reposition();
            yield return StartCoroutine(box.CloseBox(selected));
            box.anim.SetTrigger("Success");
            bank.Success(box.k);
            boxes.Remove(box);
            Debug.Log("Hardcoded value here");
            bank.Completion += 0.25f / 5f;
            if (boxes.Count == 0)
            {
                onComplete.Invoke();
                foreach (var item in items)
                {
                    Destroy(item);
                }
            }
        }
        else
        {
            if (selected.errors.Contains(box) == false)
            {
                selected.errors.Add(box);
            }
            box.anim.SetTrigger("Fail");
            bank.Failure(box.k);
            bank.ChangeXP(selected.k, -1);
        }
    }
}
