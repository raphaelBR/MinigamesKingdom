using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Creates cards based off a list.
/// </summary>
public class CardPack : MonoBehaviour
{
    public Card cardPrefab;
    public Transform cardParent;
    public RectTransform bigSpot;
    public CustomAnimation blocker;
    public CustomAnimation previous;
    public CustomAnimation next;

    [HideInInspector]
    public Card activeCard;
    [HideInInspector]
    public List<Card> cards = new List<Card>();

    public void SetDisplay(string key, bool state)
    {
        var c = cards.Find(w => w.key == key);
        if (c != null)
        {
            c.gameObject.SetActive(state);
            if (state == true)
            {
                c.Init(key, this);
            }
        }
    }

    public void SemiClear()
    {
        if (activeCard != null)
        {
            activeCard.Reset();
            
            activeCard = null;
        }
        blocker.Teleport(0);
    }

    public void Clear()
    {
        if (activeCard != null)
        {
            Destroy(activeCard.activeDummy);
            Destroy(activeCard);
        }
        foreach (Card c in cards)
        {
            Destroy(c.gameObject);
        }
        cards.Clear();
        SetCardActive(null);
    }

    public void Change(int i)
    {
        int result = cards.IndexOf(activeCard) + i;
        if (result < 0 || result >= cards.Count)
        {
            return;
        }
        var c = cards[result];
        activeCard.ToggleDisplay();
        c.ToggleDisplay();
    }

    public void Spawn(string key)
    {
        var c = Instantiate(cardPrefab, cardParent);
        c.Init(key, this);
        cards.Add(c);
    }

    public void SetCardActive(Card c)
    {
        activeCard = c;
        
        if (activeCard != null)
        {
            var i = cards.IndexOf(activeCard);
            blocker.GoTo(1);
            if (i > 0)
            {
                previous.GoTo(1);
            }
            else
            {
                previous.GoTo(0);
            }
            if (i < cards.Count - 1)
            {
                next.GoTo(1);
            }
            else
            {
                next.GoTo(0);
            }
        }
        else
        {
            blocker.GoTo(0);
        }
    }

}
