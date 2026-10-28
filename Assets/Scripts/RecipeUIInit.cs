using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUIInit : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI recipeNameObj;
    [SerializeField] Image recipeIco;
    RecipeObj recipe;

    public void Initialize(RecipeObj assignedRecipe)
    {
        recipe = assignedRecipe;    
        recipeNameObj.text = recipe.Output.ItemName;
        recipeIco.sprite = recipe.Output.ItemIcon;
    }
}
