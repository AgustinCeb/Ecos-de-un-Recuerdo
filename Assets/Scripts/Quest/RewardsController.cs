using UnityEngine;
using UnityEngine.Rendering;

public class RewardsController : MonoBehaviour
{
    public static RewardsController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GiveQuestRewards(QuestData quest, Inventory inventory, Inventory2 inventory2, Inventory3 inventory3)
    {
        if (quest?.questReward == null) return;

        foreach (var reward in quest.questReward)
        {
            switch (reward.type)
            {
                case RewardType.Item:
                    //Give Item
                    GiveItemReward(inventory, inventory2, inventory3 ,reward.rewardID, reward.amount);
                    break;

                case RewardType.Exp: 
                    //Give Exp
                    break;

                case RewardType.Custom: 
                    //Custom
                    break;

            }
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
                inventory2.addItem(itemReward, amount); 
                break;

        }
        
    }

}
