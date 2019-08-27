using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct RectPair
{
    public RectTransform first;
    public RectTransform second;
}

public class Picker : Practice
{
    public RectTransform itemIn;
    public RectTransform itemOut;

    public RectTransform boxBeforeFirst;
    public RectTransform boxBefore;
    public GameObject boxSpot;

    public Transform itemParent;
    public PickerItem itemPrefab;
    public Transform boxParent;
    public Transform boxHolder;
    public PickerBox boxPrefab;
    public int gridSpots = 12;
    public int total = 16;

    List<string> keys = new List<string>();
    List<PickerItem> items = new List<PickerItem>();
    List<PickerBox> boxes = new List<PickerBox>();
    int progress = 0;
    List<PickerBox> errors = new List<PickerBox>();
    List<RectPair> boxIn = new List<RectPair>();

    private void Start()
    {
        generator.Init();
        for (int i = 0; i < gridSpots; i++)
        {
            var g = Instantiate(boxSpot, boxParent);
            boxIn.Add(new RectPair() { first = g.GetComponent<RectTransform>(), second = g.transform.GetChild(0).GetComponent<RectTransform>() });
        }
        boxIn.Shuffle();

        keys = generator.Generate(total);
        for (int i = 0; i < gridSpots; i++)
        {
            var it = Instantiate(itemPrefab, itemParent);
            it.Init(keys[i], itemIn, itemOut);
            items.Add(it);
            it.transform.SetAsFirstSibling();
            var b = Instantiate(boxPrefab, boxHolder);
            b.Init(keys[i]);
            b.anim.state[0].status = boxBeforeFirst;
            b.anim.state[1].status = boxIn[i].first;
            b.anim.state[2].status = boxIn[i].second;
            b.anim.Play(0, 1);
            boxes.Add(b);
            b.butn.onClick.AddListener(delegate { Test(b); });
        }
        foreach (PickerBox box in boxes)
        {
            box.transform.SetSiblingIndex(Random.Range(0, boxes.Count));
        }
    }

    public void Test(PickerBox b)
    {
        if (b.key == items[progress].key)
        {
            RectTransform first = b.anim.state[1].status;
            RectTransform second = b.anim.state[2].status;
            bank.Success(b.key);
            items[progress].Win();
            boxes.Remove(b);
            b.Close();
            foreach (PickerBox box in boxes)
            {
                box.Clear();
            }
            progress++;
            bank.Completion = (float)progress / total;
            if (progress == total)
            {
                bank.EndGame(true);
            }
            else if (items.Count < total)
            {
                var it = Instantiate(itemPrefab, itemParent);
                it.Init(keys[gridSpots + progress- 1], itemIn, itemOut);
                items.Add(it);
                it.transform.SetAsFirstSibling();
                var bo = Instantiate(boxPrefab, boxHolder);
                bo.Init(keys[gridSpots + progress - 1]);
                bo.anim.state[0].status = boxBefore;
                bo.anim.state[1].status = first;
                bo.anim.state[2].status = second;
                bo.anim.state[1].delay = 0.5f;
                bo.anim.Teleport(0);
                bo.anim.Play(0, 1);
                boxes.Add(bo);
                bo.butn.onClick.AddListener(delegate { Test(bo); });
            }
        }
        else
        {
            bank.Failure(b.key);
            b.Error();
        }
    }

}
