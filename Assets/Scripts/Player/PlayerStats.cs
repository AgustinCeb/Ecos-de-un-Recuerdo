using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    int playerLevel;
    [SerializeField]int[] levelList;
    int requiredXP;
    int heldXP;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        InitializeXP();
    }
    public void AddXP(int addXp)
    {
        heldXP += addXp;
        if (heldXP >= requiredXP)
        {
            LevelUp();
        }
        Debug.Log("Nivel "+playerLevel+ " Experiencia para el siguiente nivel: "+ (requiredXP-heldXP));
    }
    void LevelUp()
    {
        if (playerLevel < levelList.Length)
        {
            heldXP -= levelList[playerLevel];
            playerLevel++;
            requiredXP = levelList[playerLevel];
            Debug.Log("LEVEL UP");
        }

    }
    void InitializeXP()
    {
        playerLevel = 1;
        heldXP = 0;
        requiredXP = levelList[playerLevel];
        AddXP(0);
    }
}
