using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    
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

    public System.Action<int> OnLevelChanged;

    private void Awake()
    {
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
        GameManager.Instance.ShowNotification("Obtubiste"+ " " + addXp +" "+"Exp");
        
    }
    void LevelUp()
    {
        
            heldXP -= LevelStats.requiredExpForNextLevel;
            playerLevel++;
            OnLevelChanged?.Invoke(playerLevel);
            LevelStats = statValuesPerLevel.levels[playerLevel-1];
            requiredXP = LevelStats.requiredExpForNextLevel;

        //Recarga la vida al subir de nivel
        PlayerHealt playerHealt = GetComponent<PlayerHealt>();

        if (playerHealt != null)
        {
            playerHealt.RestoreHealth();
        }

        // Recargar maná al subir de nivel
        PlayerMana playerMana = GetComponent<PlayerMana>();

        if (playerMana != null)
        {
            playerMana.RestoreMana();
        }

        Debug.Log("LEVEL UP, YOUR DAMAGE IS NOW "+LevelStats.damage+ " ,YOUR HEALTH IS NOW " + LevelStats.maxHealth+ " AND YOUR DEFENSE IS NOW " + LevelStats.defense);
        GameManager.Instance.ShowNotification("¡Has subido de nivel!");
        GameManager.Instance.ShowNotification("Max Healt" + " " + LevelStats.maxHealth +" "+ "Max Mana" + " " + LevelStats.Maxmana);
        GameManager.Instance.ShowNotification("Ataque" + " " + LevelStats.damage + " " + "DEF" + " " + LevelStats.defense + "" + "Agi" + " " + LevelStats.agility);
        GameManager.Instance.ShowNotification("Nivel " + " " + playerLevel + " " + " Experiencia para el siguiente nivel: " + " " + " " + (requiredXP - heldXP));

    }
    void InitializeXP()
    {
        Debug.Log("InitializeXP ejecutado: nivel inicial 1");

        playerLevel = 1;
        heldXP = 0;

        LevelStats = statValuesPerLevel.levels[playerLevel -1];
        requiredXP = LevelStats.requiredExpForNextLevel;
        
    }

    public void SaveData(PlayerData data)
    {
        data.PlayerLevel = playerLevel;
        data.HeldXP = heldXP;
    }

    public void LoadData(PlayerData data)
    {
        playerLevel = Mathf.Clamp( data.PlayerLevel,1,statValuesPerLevel.levels.Length);

        LevelStats = statValuesPerLevel.levels[playerLevel - 1];
        OnLevelChanged?.Invoke(playerLevel);

        requiredXP = LevelStats.requiredExpForNextLevel;

        heldXP = Mathf.Max(0, data.HeldXP);

        Debug.Log("Estadísticas restauradas. Nivel: " + playerLevel);
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

    public int getBaseHealthPoints() => Convert.ToInt32(LevelStats.maxHealth);
    public int getBaseMaxMana() => Convert.ToInt32(LevelStats.Maxmana);
    public int getBaseDamage() => Convert.ToInt32(LevelStats.damage);
    public int getBaseDefense() => Convert.ToInt32(LevelStats.defense);
    public int getBaseAgility() => Convert.ToInt32(LevelStats.agility);

    public void alterExStats(float addHp, float addMana, float addDmg, float addDef, float addAgi)
    {
        exMaxHp += addHp;
        exMaxMana += addMana;
        exDamage += addDmg;
        exDefense += addDef;
        exAgility += addAgi;
    }

    public void SetRuneStats(float hp, float mana, float damage, float defense, float agility)
    {
        exMaxHp = hp;
        exMaxMana = mana;
        exDamage = damage;
        exDefense = defense;
        exAgility = agility;
    }

}
