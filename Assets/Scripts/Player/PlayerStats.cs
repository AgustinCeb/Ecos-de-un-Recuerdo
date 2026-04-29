using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    int playerLevel;
    [SerializeField] StatsPerLevel statValuesPerLevel;
    int requiredXP;
    int heldXP;
    LevelStats LevelStats;

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
        if (playerLevel < statValuesPerLevel.levels.Length)
        {
            heldXP -= LevelStats.requiredExpForNextLevel;
            playerLevel++;
            LevelStats = statValuesPerLevel.levels[playerLevel-1];
            requiredXP = LevelStats.requiredExpForNextLevel;
            Debug.Log("LEVEL UP, YOUR DAMAGE IS NOW "+LevelStats.damage+ " ,YOUR HEALTH IS NOW " + LevelStats.maxHealth+ " AND YOUR DEFENSE IS NOW " + LevelStats.defense);
        }

    }
    void InitializeXP()
    {
        playerLevel = 1;
        heldXP = 0;
        LevelStats = statValuesPerLevel.levels[playerLevel -1];
        requiredXP = LevelStats.requiredExpForNextLevel;
        AddXP(0);
    }

}
