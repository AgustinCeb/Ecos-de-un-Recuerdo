using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "ScriptableObject/Recipe")]
public class RecipeObj : ScriptableObject
{
    public ItemData[] Input;
    public ItemData Output;
}
