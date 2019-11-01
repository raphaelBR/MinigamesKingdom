using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[System.Serializable]
public class ExperienceDictionary : SerializableDictionaryBase<string, Experience> { }

[System.Serializable]
public class FavoritesDictionary : SerializableDictionaryBase<string, bool> { }


[System.Serializable]
public class Progression
{
    [SerializeField]
    public FavoritesDictionary wordsFavorites = new FavoritesDictionary();
    [SerializeField]
    public ExperienceDictionary wordsMastery = new ExperienceDictionary();
    [SerializeField]
    public Experience userMastery = new Experience();
    [SerializeField]
    public int coins = 0;
    [SerializeField]
    public int keys = 0;
}