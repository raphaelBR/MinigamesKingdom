using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    Carrot,
    Onion,
    Tomato,
    Salad,
    Chicken
}

[System.Serializable]
public class Recipe
{
    public string name;
    public List<Ingredient> ingredients;
}

[System.Serializable]
public class Ingredient
{
    public IngredientType type;
    public bool sliced;
    public bool cooked;


    string Translate(IngredientType type)
    {
        switch (type)
        {
            case IngredientType.Carrot:
                return Dico.Locale("carrot");
            case IngredientType.Onion:
                return Dico.Locale("onion");
            case IngredientType.Tomato:
                return Dico.Locale("tomato");
            case IngredientType.Salad:
                return Dico.Locale("salad");
            case IngredientType.Chicken:
                return Dico.Locale("chicken");
            default:
                return "?";
        }
    }
}

public class CookingManager : MonoBehaviour {

    public List<Recipe> recipes;

}
