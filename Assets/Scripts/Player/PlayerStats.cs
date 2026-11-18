using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
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

    public NetworkVariable<int> PlayerLevel = new(1);

    public NetworkVariable<int> HeldXP = new(0);

    public int _playerLevel => playerLevel;

    public System.Action<int> OnLevelChanged;

    
    private void Awake()
    {
        InitializeXP();
    }

    public override void OnNetworkSpawn()
    {
        PlayerLevel.OnValueChanged += OnPlayerLevelChanged;

        if (IsServer)
        {
            // El servidor conserva los datos restaurados.
            // Los publica para que los clientes los reciban.
            PlayerLevel.Value = playerLevel;
            HeldXP.Value = heldXP;
        }
        else
        {
            // El cliente recibe los datos del servidor.
            playerLevel = PlayerLevel.Value;
            heldXP = HeldXP.Value;

            LevelStats = statValuesPerLevel.levels[playerLevel - 1];
            requiredXP = LevelStats.requiredExpForNextLevel;
        }
    }

    public void AddXP(int addXp)
    {
       
        if (addXp <= 0) return;

        if (IsServer)
        {
            AddXPServer(addXp);
        }
        else if (IsOwner)
        {
            AddXPRpc(addXp);
        }
    }

    ///
    [Rpc(SendTo.Server)]
    private void AddXPRpc(int addXp)
    {
        if (addXp <= 0) return;

        AddXPServer(addXp);
    }

    private void AddXPServer(int addXp)
    {
        if (!IsServer) return;

        heldXP += addXp;

        while (heldXP >= requiredXP && playerLevel < statValuesPerLevel.levels.Length)
        {
            LevelUp();
        }

        PlayerLevel.Value = playerLevel;
        HeldXP.Value = heldXP;

        Debug.Log("Nivel " + playerLevel + " Experiencia para el siguiente nivel: " + (requiredXP - heldXP));

        if (IsOwner)
        {
            ShowXPNotificationRpc(addXp);
        }
        else
        {
            ShowXPNotificationRpc(addXp);
        }
    }
     
    [Rpc(SendTo.Owner)]
    private void ShowXPNotificationRpc(int addXp)
    {
        GameManager.Instance.ShowNotification("Obtuviste " + addXp + " Exp");
    }
    //

    void LevelUp()
    {

        if (!IsServer) return;

        heldXP -= requiredXP;
        playerLevel++;

        LevelStats = statValuesPerLevel.levels[playerLevel - 1];
        requiredXP = LevelStats.requiredExpForNextLevel;

        // Sincronizar nivel y experiencia.
        PlayerLevel.Value = playerLevel;
        HeldXP.Value = heldXP;

        // Recargar vida y maná desde el servidor.
        PlayerHealt playerHealt = GetComponent<PlayerHealt>();
        if (playerHealt != null)
            playerHealt.RestoreHealth();

        PlayerMana playerMana = GetComponent<PlayerMana>();
        if (playerMana != null)
            playerMana.RestoreMana();

        // Notificaciones únicamente para el propietario.
        ShowLevelUpNotificationRpc(
            playerLevel,
            LevelStats.maxHealth,
            LevelStats.Maxmana,
            LevelStats.damage,
            LevelStats.defense,
            LevelStats.agility,
            requiredXP - heldXP
        );

        Debug.Log("LEVEL UP. Nivel actual: " + playerLevel);

        
    }

    //
    [Rpc(SendTo.Owner)]
    private void ShowLevelUpNotificationRpc(
    int level,
    float health,
    float mana,
    float damage,
    float defense,
    float agility,
    int remainingXP)
    {
        GameManager.Instance.ShowNotification("¡Has subido de nivel!");
        GameManager.Instance.ShowNotification("Max Health " + health + " Max Mana " + mana);
        GameManager.Instance.ShowNotification("Ataque " + damage + " DEF " + defense + " Agi " + agility);
        GameManager.Instance.ShowNotification("Nivel " + level + " Experiencia para el siguiente nivel: " + remainingXP);
    }
    //

    private void OnPlayerLevelChanged(int previousValue, int newValue)
    {
        playerLevel = newValue;

        LevelStats = statValuesPerLevel.levels[playerLevel - 1];
        requiredXP = LevelStats.requiredExpForNextLevel;

        OnLevelChanged?.Invoke(playerLevel);
    }

    void InitializeXP()
    {
        
        playerLevel = 1;
        heldXP = 0;

        LevelStats = statValuesPerLevel.levels[playerLevel -1];
        requiredXP = LevelStats.requiredExpForNextLevel;
        
    }

    public void SaveData(PlayerData data)
    {
        data.PlayerLevel = playerLevel;
        data.HeldXP = heldXP;
        Debug.Log("GUARDANDO NIVEL: " + data.PlayerLevel);
    }

    public void LoadData(PlayerData data)
    {
        playerLevel = Mathf.Clamp( data.PlayerLevel,1,statValuesPerLevel.levels.Length);

        LevelStats = statValuesPerLevel.levels[playerLevel - 1];
        OnLevelChanged?.Invoke(playerLevel);

        requiredXP = LevelStats.requiredExpForNextLevel;

        heldXP = Mathf.Max(0, data.HeldXP);

       
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
