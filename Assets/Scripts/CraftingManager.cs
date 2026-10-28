using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] RecipeObj [] recipeList;
    [SerializeField] Transform contentObj;
    [SerializeField] GameObject recipeBtn;
    [SerializeField] Image[] inputImages;
    [SerializeField] Image outputImage;
    [SerializeField] TextMeshProUGUI recipeNameTMPro;
    RecipeObj activeRecipe;

    public GameObject player;

    private void Awake()
    {
        activeRecipe = null;
        foreach (RecipeObj r in recipeList)
        {
            GameObject recipe =Instantiate(recipeBtn, contentObj);
            RecipeUIInit recipeInit = recipe.GetComponent<RecipeUIInit>();
            recipeInit.Initialize(r);
            recipe.GetComponent<Button>().onClick.AddListener(() => recipeButtonPress(r)); 
        }

    }
    void recipeButtonPress(RecipeObj recipe)
    {
        for (int i = 0; i < inputImages.Length; i++)
        {
            inputImages[i].sprite = recipe.Input[i].ItemIcon;    
        }
        outputImage.sprite = recipe.Output.ItemIcon;
        recipeNameTMPro.text = recipe.Output.ItemName;
    }
    public void CraftButton()
    {
        Inventory2 inv = player.GetComponent<Inventory2>();
        inv.addItem(recipeList[0].Output,1);
    }
}
