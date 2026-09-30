using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    int playerLevel;
    [SerializeField] StatsPerLevel statValuesPerLevel;
    int requiredXP;
    int heldXP;
    LevelStats LevelStats;
    float exMaxHp;
    float exMaxMana;
    float exDamage;
    float exDefense;
    float exAgility;

    public int _playerLevel => playerLevel;

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
        while (heldXP >= requiredXP && playerLevel< statValuesPerLevel.levels.Length)
        {
            LevelUp();
        }
        Debug.Log("Nivel "+playerLevel+ " Experiencia para el siguiente nivel: "+ (requiredXP-heldXP));
    }
    void LevelUp()
    {
        
            heldXP -= LevelStats.requiredExpForNextLevel;
            playerLevel++;
            LevelStats = statValuesPerLevel.levels[playerLevel-1];
            requiredXP = LevelStats.requiredExpForNextLevel;

        // Recargar maná al subir de nivel
        PlayerMana playerMana = GetComponent<PlayerMana>();

        if (playerMana != null)
        {
            playerMana.RestoreMana();
        }

        Debug.Log("LEVEL UP, YOUR DAMAGE IS NOW "+LevelStats.damage+ " ,YOUR HEALTH IS NOW " + LevelStats.maxHealth+ " AND YOUR DEFENSE IS NOW " + LevelStats.defense);


    }
    void InitializeXP()
    {
        playerLevel = 1;
        heldXP = 0;
        LevelStats = statValuesPerLevel.levels[playerLevel -1];
        requiredXP = LevelStats.requiredExpForNextLevel;
        AddXP(0);
    }
    public int getHealthPoints()
    {
        int hp=0;
        hp = Convert.ToInt32(LevelStats.maxHealth) +Convert.ToInt32(exMaxHp);
        return hp;

    }
    public int getMaxMana()
    {
        int mana = 0;
        mana = Convert.ToInt32(LevelStats.Maxmana) + Convert.ToInt32(exMaxMana);
        return mana;
    }
    public int getDamage()
    {
        int damage = 0;
        damage = Convert.ToInt32(LevelStats.damage)+ Convert.ToInt32(exDamage);
        return damage;
    }
    public int getDefense()
    {
        int def = 0;
        def = Convert.ToInt32((LevelStats.defense))+ Convert.ToInt32(exDefense);
        return def;
    }
    public int getAgility()
    {
        int agility = 0;
        agility = Convert.ToInt32(LevelStats.agility)+ Convert.ToInt32(exAgility);
        return agility;
    }
    public void alterExStats(float addHp, float addMana, float addDmg, float addDef, float addAgi)
    {
        exMaxHp += addHp;
        exMaxMana += addMana;
        exDamage += addDmg;
        exDefense += addDef;
        exAgility += addAgi;
    }
}
