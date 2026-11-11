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

    private Inventory2 inventory2;
    private Inventory3 inventory3;

    private void Awake()
    {
       
        activeRecipe = null;

        inventory2 = player.GetComponent<Inventory2>();
        inventory3 = player.GetComponent<Inventory3>();

        if (inventory2 == null || inventory3 == null)
        {
            Debug.LogError("No se encontraron los inventarios del jugador.");
            return;
        }

        foreach (RecipeObj r in recipeList)
        {
            GameObject recipe = Instantiate(recipeBtn, contentObj);

            RecipeUIInit recipeInit = recipe.GetComponent<RecipeUIInit>();
            recipeInit.Initialize(r);

            recipe.GetComponent<Button>().onClick.AddListener(() => recipeButtonPress(r));

        }

    }

    void recipeButtonPress(RecipeObj recipe)
    {
        activeRecipe = recipe;

        Debug.Log("Receta asignada: " + activeRecipe.name);
        Debug.Log("Instancia: " + GetInstanceID());

        for (int i = 0; i < inputImages.Length; i++)
        {
            inputImages[i].sprite = recipe.Input[i].ItemIcon;
        }

        outputImage.sprite = recipe.Output.ItemIcon;
        recipeNameTMPro.text = recipe.Output.ItemName;
    }

    public void CraftButton()
    {
        Debug.Log("Receta actual: " + (activeRecipe != null ? activeRecipe.name : "NULL"));
        Debug.Log("Instancia: " + GetInstanceID());


        if (activeRecipe == null)
        {
            Debug.Log("No hay ninguna receta seleccionada.");
            return;
        }

        // Comprobar que tenemos todos los materiales.
        foreach (ItemData item in activeRecipe.Input)
        {
            if (item == null)
                continue;

            if (inventory2.GetItemCount(item) < 1)
            {
                Debug.Log("No tienes todos los materiales.");
                return;
            }
        }

        // Quitar los materiales.
        foreach (ItemData item in activeRecipe.Input)
        {
            if (item == null)
                continue;

            inventory2.RemoveItem(item, 1);
        }

        // Agregar el resultado al inventario de consumibles.
        inventory3.addItem(activeRecipe.Output, 1);

        Debug.Log("Objeto fabricado.");
    }


}



