using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;

public class RewardsController : MonoBehaviour
{
    public static RewardsController Instance { get; private set; }

    private ItemDictionary _itemDictionary;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _itemDictionary = FindAnyObjectByType<ItemDictionary>();

    }

    public void GiveQuestRewards(QuestData quest, Inventory inventory, Inventory2 inventory2, Inventory3 inventory3)
    {
        Debug.Log("Inventory: " + (inventory != null));
        Debug.Log("Inventory2: " + (inventory2 != null));
        Debug.Log("Inventory3: " + (inventory3 != null));

        NetworkObject playerNetworkObject = null;

        if (inventory != null)
            playerNetworkObject = inventory.GetComponent<NetworkObject>();

        if (playerNetworkObject == null && inventory2 != null)
            playerNetworkObject = inventory2.GetComponent<NetworkObject>();

        if (playerNetworkObject == null && inventory3 != null)
            playerNetworkObject = inventory3.GetComponent<NetworkObject>();

        if (playerNetworkObject == null || !playerNetworkObject.IsOwner)
            return;
        if (quest?.questReward == null) return;

        PlayerStats stats = null;

        if (inventory != null)
            stats = inventory.GetComponent<PlayerStats>();

        if (stats == null && inventory2 != null)
            stats = inventory2.GetComponent<PlayerStats>();

        if (stats == null && inventory3 != null)
            stats = inventory3.GetComponent<PlayerStats>();

        List<QuestReward> itemRewards = new List<QuestReward>();

        foreach (var reward in quest.questReward)
        {
            switch (reward.type)
            {
                case RewardType.Item:
                    
                    if (quest.useRandomItemReward)
                    {
                        itemRewards.Add(reward);
                    }
                    else
                    {
                        GiveItemReward(inventory, inventory2,inventory3, reward.rewardID, reward.amount);
                    }

                    break;

                case RewardType.Exp:

                    if (stats != null)
                    {
                        stats.AddXP(reward.amount);
                        Debug.Log("Experiencia entregada: " + reward.amount);
                    }
                    else
                    {
                        Debug.LogWarning("No se encontró PlayerStats para entregar experiencia.");
                    }

                    break;

                case RewardType.Custom: 
                    //Custom
                    break;

            }


        }

        if (quest.useRandomItemReward && itemRewards.Count > 0)
        {
            GiveRandomItemReward( itemRewards,inventory);
        }

    }

    public void GiveItemReward(Inventory inventory, Inventory2 inventory2, Inventory3 inventory3, int itemID, int amount)
    {
        var itemReward = FindAnyObjectByType<ItemDictionary>()?.GetItem(itemID);

        if(itemReward == null) return;

        switch (itemReward.Type)
        {
            case ItemData.ItemType.Equipment:
                inventory.addItem(itemReward, amount);
                break;

            case ItemData.ItemType.Material:
                inventory2.addItem(itemReward, amount);
                break;

            case ItemData.ItemType.Consumable: 
                inventory3.addItem(itemReward, amount); 
                break;

        }
        
    }

    private void GiveRandomItemReward(List<QuestReward> rewards,Inventory inventory)
    {
        if (inventory == null)
        {
            Debug.LogWarning("No se encontró el inventario de runas.");
            return;
        }

        if (!inventory.HasFreeSlot)
        {
            Debug.LogWarning("Inventario lleno. No se pudo entregar la recompensa.");
            return;
        }

        List<QuestReward> availableRewards = new List<QuestReward>();

        foreach (var reward in rewards)
        {
            ItemData item = _itemDictionary.GetItem(reward.rewardID);

            if (item == null) continue;

            // Este modo aleatorio está pensado para runas.
            if (item.Type != ItemData.ItemType.Equipment)
                continue;

            // No permitir recompensas repetidas.
            if (inventory.HasItem(item))
                continue;

            availableRewards.Add(reward);
        }

        if (availableRewards.Count == 0)
        {
            Debug.Log("No hay runas nuevas disponibles para entregar.");
            return;
        }

        QuestReward selectedReward = SelectRandomReward(availableRewards);

        if (selectedReward == null) return;

        GiveItemReward( inventory,null,null,selectedReward.rewardID,selectedReward.amount);

        Debug.Log("Runa aleatoria entregada. ID: " + selectedReward.rewardID);
    }

    private QuestReward SelectRandomReward(List<QuestReward> rewards)
    {
        float totalWeight = 0f;

        foreach (var reward in rewards)
        {
            totalWeight += Mathf.Max(0f, reward.chance);
        }

        // Si todas tienen probabilidad cero, elegir una al azar.
        if (totalWeight <= 0f)
        {
            return rewards[Random.Range(0, rewards.Count)];
        }

        float randomValue = Random.Range(0f, totalWeight);

        foreach (var reward in rewards)
        {
            randomValue -= Mathf.Max(0f, reward.chance);

            if (randomValue <= 0f)
                return reward;
        }

        return rewards[rewards.Count - 1];
    }

}
