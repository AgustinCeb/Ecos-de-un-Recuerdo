using UnityEngine;
using Unity.Netcode;

public class PlayerHealt : NetworkBehaviour
{
    int maxHealth;

    public NetworkVariable <int> Healt = new(100);

    public NetworkVariable<int> ShieldHits = new(0);

    public NetworkVariable<bool> ShieldActivate = new(false);

    public override void OnNetworkSpawn()
    {
        if (!IsServer)return;

        if (IsOwner && DeathScreenUI.Instance != null)
        {
            DeathScreenUI.Instance.HideDeathScreen();
        }

        int startingMaxHp = maxHealth;
        maxHealth = GetComponent<PlayerStats>().getHealthPoints();
        
    }

    private void Update()
    {
        if (!IsServer) return;

        int newMaxHealth = GetComponent<PlayerStats>().getHealthPoints();

        if (newMaxHealth != maxHealth)
        {
            maxHealth = newMaxHealth;

            // Solo reducir la vida si supera el nuevo máximo.
            if (Healt.Value > maxHealth)
            {
                Healt.Value = maxHealth;
            }
        }
    }

    public void RestoreHealth()
    {
        if (!IsServer) return;

        Healt.Value = maxHealth;
    }

    public void TakeDamagePlayer(int enemyDamage)
    {
        if (!IsServer) return;

        if (ShieldActivate.Value)
        {
            ShieldHits.Value--;

            Debug.Log("Golpe bloqueado. Escudo restante: " + ShieldHits.Value);

            if (ShieldHits.Value <= 0)
            {
                ShieldHits.Value = 0;
                ShieldActivate.Value = false;
            }

            return;
        }

        // Obtener defensa porcentual.
        float defense = GetComponent<PlayerStats>().getDefense();

        // Limitar la defensa entre 0% y 100%.
        defense = Mathf.Clamp(defense, 0f, 100f);

        // Calcular daño después de aplicar la defensa.
        int finalDamage = Mathf.CeilToInt(enemyDamage * (1f - defense / 100f));

        Debug.Log("Daño enemigo: " + enemyDamage + " | Defensa: " + defense + "%" + " | Daño recibido: " + finalDamage);

        Healt.Value -= finalDamage;

        if (Healt.Value <= 0)
        {
            Healt.Value = 0;

            SavePlayerData();
            ShowDeathScreenRpc();

            NetworkObject.Despawn();
        }
    }

    public bool HealPlayer(int amount)
    {
        if (!IsServer) return false;


        if (Healt.Value >= maxHealth)
            return false;

        Healt.Value = Mathf.Min(Healt.Value + amount, maxHealth);

        return true;
    }

    private void SavePlayerData()
    {
        if (!IsServer) return;

        PlayerDataManager dataManager = PlayerDataManager.Instance;

        if (dataManager == null)
        {
            Debug.LogError("No se encontró PlayerDataManager. No se pudieron guardar los datos.");
            return;
        }

        PlayerData data = dataManager.GetOrCreateData(OwnerClientId);

        if (data == null) return;

        // Guardar nivel y experiencia.
        GetComponent<PlayerStats>().SaveData(data);

        // Guardar inventarios.
        GetComponent<Inventory>().SaveInventory(data);
        GetComponent<Inventory2>().SaveInventory(data);
        GetComponent<Inventory3>().SaveInventory(data);

        // Guardar runas equipadas.
        GetComponent<RuenEquip>().SaveRunes(data);

        Debug.Log("Datos guardados antes de morir. Jugador: " + OwnerClientId);
    }

    [Rpc(SendTo.Owner)]
    private void ShowDeathScreenRpc()
    {
        if (DeathScreenUI.Instance == null)
        {
            Debug.LogError("DeathScreenUI todavía no está registrado.");
            return;
        }

        DeathScreenUI.Instance.ShowDeathScreen();
    }

}
