using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }

    public List<QuestProgress> activeQuest = new();
    private QuestUI questUI;

    private Inventory2 subscribedInventory;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        questUI = FindObjectOfType<QuestUI>();

    }

    public void AcceptQuest(QuestData quest, Inventory2 inventory)
    {
        if(quest == null || inventory == null)
        {
            Debug.LogWarning("No se pudo aceptar Falta Mision o Inventario");
            return;
        }

        SubscribeToInventory(inventory);

        if (IsQuestActive(quest.questId)) 
        {
            RefreshCollectObjectives();
            return;
        };

        activeQuest.Add(new QuestProgress(quest));

        RefreshCollectObjectives();

    }

    private void SubscribeToInventory(Inventory2 inventory)
    {
        
        if (subscribedInventory == inventory) return;

        if(subscribedInventory != null)
        {
            subscribedInventory.OnInventoryChanged -= RefreshCollectObjectives;
        }

        subscribedInventory = inventory;

        subscribedInventory.OnInventoryChanged += RefreshCollectObjectives;

    }

    private void RefreshCollectObjectives()
    {
        
        if (subscribedInventory == null) return;

        foreach(var quest in activeQuest)
        {
            foreach(var obj in quest.objetives)
            {

                if(obj.type != ObjectiveType.CollecItem) continue;

                if(obj.requiredItem == null) continue;

                int amount = subscribedInventory.GetItemCount(obj.requiredItem);

                obj.currentAmount = Mathf.Min(amount, obj.requiredAmount);

            }
        }

        questUI?.UpdateQuestUI();
    }



    public bool IsQuestActive(string questID) 
    {
        return activeQuest.Exists(q => q.QuestID == questID);
    }

    private void OnDestroy()
    {
        if(subscribedInventory != null)
        {
            subscribedInventory.OnInventoryChanged -= RefreshCollectObjectives;
        }

        if(Instance == this)
        {
            Instance = null;
        }

    }

}
