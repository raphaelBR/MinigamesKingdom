using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using UnityEngine.Events;

public static class UISetExtensions
{
    static MethodInfo toggleSetMethod;

    static UISetExtensions()
    {
        MethodInfo[] methods = typeof(Toggle).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
        for (var i = 0; i < methods.Length; i++)
        {
            if (methods[i].Name == "Set" && methods[i].GetParameters().Length == 2)
            {
                toggleSetMethod = methods[i];
                break;
            }
        }
    }
    public static void SilentSet(this Toggle instance, bool value)
    {
        toggleSetMethod.Invoke(instance, new object[] { value, false });
    }
}

[Serializable]
public class ToggleBundle
{
    [HideInInspector]
    public List<int> filter = new List<int>();
    public List<Toggle> members = new List<Toggle>();
    public Toggle any;
    public UnityEvent onFilter;

    public void Init()
    {
        any.isOn = true;
    }

    public void Filter(Toggle b)
    {
        any.interactable = true;
        if (b.isOn)
        {
            filter.Add(members.IndexOf(b) + 1);
        }
        else
        {
            filter.Remove(members.IndexOf(b) + 1);
        }

        bool test = true;
        foreach (Toggle p in members)
        {
            if (p.isOn != b.isOn)
            {
                test = false;
            }
        }
        any.isOn = test;
        onFilter.Invoke();
    }

    public void Filter()
    {
        if (any.isOn)
        {
            foreach (Toggle p in members)
            {
                p.SilentSet(false);
            }
            filter.Clear();
            any.interactable = false;
        }
        onFilter.Invoke();
    }
}

/// <summary>
/// The interface that lets the player consult his collection.
/// </summary>
public class Collection : MonoBehaviour
{
    [Header("Spawn")]
    public CardPack cardPack;
    public Transform packParent;
    public Toggle packPrefab;
    [Header("Filters")]
    public ToggleBundle pack;
    public ToggleBundle rarity;
    public ToggleBundle level;
    public InputField textFilter;
    [Header("Anims")]
    public CustomAnimation favorites;
    public List<CustomAnimation> advanced;
    public CustomAnimation noResults;

    bool advancedFilter = false;
    bool favoritesFilter = false;
    List<string> packsFilter = new List<string>();
    string testString = "";

    private void Start()
    {
        StartCoroutine(Init());
    }

    public IEnumerator Init()
    {
        // Clear all existing packs
        foreach (Toggle p in pack.members)
        {
            p.onValueChanged.RemoveAllListeners();
            Destroy(p.gameObject);
            yield return null;
        }
        pack.members.Clear();

        // Create toggle packs
        foreach (string item in Account.unlocks.packs)
        {
            var p = Instantiate(packPrefab, packParent);
            p.name = item;
            p.onValueChanged.AddListener(delegate { FilterPack(item, p); });
            p.GetComponentInChildren<Text>().text = Dico.FirstCharToUpper(item);
            foreach (Transform i in p.transform)
            {
                if (i.name == "Back")
                {
                    i.GetComponent<Image>().sprite = Dico.GetPackage(item).booster;
                }
            }
            pack.members.Add(p);
            packsFilter.Add(item);
            Dico.AddPack(item);
            yield return null;
        }

        // Create all cards
        cardPack.Clear();
        foreach (KeyValuePair<string, DicoItem> item in Dico.dico.OrderBy(w => w.Value.foreignText))
        {
            cardPack.Spawn(item.Key);
            yield return null;
        }

        foreach (var item in advanced)
        {
            item.Teleport(0);
        }
        SetAdvancedFilters(false);
        favorites.Teleport(0);
        favoritesFilter = false;
        noResults.Teleport(0);
        textFilter.text = "";
        rarity.Init();
        level.Init();
        pack.Init();
        Refresh();
    }

    public void Refresh()
    {
        cardPack.SemiClear();
        bool noResult = true;
        foreach (Card item in cardPack.cards)
        {
            bool b = false;
            if (PackTest(item.key))
            {
                if (RarityTest(item.key))
                {
                    if (LevelsTest(item.key))
                    {
                        if (FavoriteTest(item.key))
                        {
                            if (TextTest(item.key))
                            {
                                b = true;
                                noResult = false;
                            }
                        }
                    }
                }
            }
            cardPack.SetDisplay(item.key, b);
        }
        if (noResult)
        {
            noResults.GoTo(1);
        }
        else
        {
            noResults.GoTo(0);
        }
    }

    bool PackTest(string item)
    {
        if (pack.any.isOn)
        {
            return true;
        }
        return packsFilter.Contains(Dico.dico[item].pack);
    }

    bool FavoriteTest(string item)
    {
        if (!favoritesFilter)
        {
            return true;
        }
        return Progress.IsFavorite(item);
    }

    bool TextTest(string item)
    {
        return (Dico.Locale(item).ToLower().Contains(testString) || Dico.Foreign(item).ToLower().Contains(testString));
    }

    bool RarityTest(string key)
    {
        if (rarity.any.isOn)
        {
            return true;
        }
        foreach (int i in rarity.filter)
        {
            if (Dico.dico[key].rarity == i)
            {
                return true;
            }
        }
        return false;
    }

    bool LevelsTest(string key)
    {
        if (level.any.isOn)
        {
            return true;
        }
        foreach (int i in level.filter)
        {
            if (Progress.GetExp(key).Level == i)
            {
                return true;
            }
        }
        return false;
    }

    public void ToggleFilter()
    {
        SetAdvancedFilters(!advancedFilter);
    }

    public void SetAdvancedFilters(bool b)
    {
        if (b == false || cardPack.activeCard != null)
        {
            foreach (var item in advanced)
            {
                item.GoTo(0);
            }
            advancedFilter = false;
        }
        else
        {
            foreach (var item in advanced)
            {
                item.GoTo(1);
            }
            advancedFilter = true;
        }
    }

    public void ToggleFavorites()
    {
        favoritesFilter = !favoritesFilter;
        if (favoritesFilter)
        {
            favorites.GoTo(1);
        }
        else
        {
            favorites.GoTo(0);
        }
        Refresh();
    }

    public void FilterText(InputField t)
    {
        testString = t.text.ToLower();
        Refresh();
    }

    public void FilterRarity(Toggle b)
    {
        rarity.Filter(b);
        Refresh();
    }

    public void FilterRarityAny()
    {
        rarity.Filter();
        Refresh();
    }

    public void FilterLevel(Toggle b)
    {
        level.Filter(b);
        Refresh();
    }

    public void FilterLevelAny()
    {
        level.Filter();
        Refresh();
    }

    public void FilterPack(string s, Toggle b)
    {
        pack.Filter(b);
        packsFilter.Clear();
        foreach (Toggle t in pack.members)
        {
            if (t.isOn)
            {
                packsFilter.Add(t.name);
            }
        }
        Refresh();
    }

    public void FilterPackAny()
    {
        pack.Filter();
        packsFilter.Clear();
        foreach (Toggle t in pack.members)
        {
            packsFilter.Add(t.name);
        }
        Refresh();
    }
}
