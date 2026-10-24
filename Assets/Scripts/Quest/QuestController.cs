using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }

    public List<QuestProgress> activeQuest = new();
    private QuestUI questUI;

    private Inventory2 subscribedInventory;

    public List<String> handingQuestIDs = new();

    private HashSet<string> completedQuestIDs = new();

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

        if (!quest.isRepeatable && completedQuestIDs.Contains(quest.questId))
        {
            
            Debug.Log("Esta misión ya fue completada y no se puede repetir.");
            return;
        }

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
    public bool IsPrincipalQuestComplete(QuestData quest)
    {

        if (quest == null || quest.isRepeatable) return false;

        return completedQuestIDs.Contains(quest.questId);
    }

    public bool IsQuestCompleted(string questId)
    {
        QuestProgress quest = activeQuest.Find(q => q.QuestID == questId);
        return quest != null && quest.objetives.TrueForAll(o => o.IsCompleted);
    }

    public void HandInQuest(string questId)
    {
        QuestProgress quest = activeQuest.Find(q => q.QuestID == questId);

        if (quest == null)
        {
            Debug.LogWarning("No se encontró la misión activa.");
            return;
        }

        if (!IsQuestCompleted(questId))
        {
            Debug.Log("Todavía faltan objetos para completar la misión.");
            return;
        }

        if (!RemoveRequiredItemsFromInventory(questId))
        {
            Debug.Log("No se pudieron entregar los objetos de la misión.");
            return;
        }

        if (!quest.quest.isRepeatable)
        {
            completedQuestIDs.Add(quest.QuestID);
        }

        // Quitar la misión de la lista de misiones activas.
        activeQuest.Remove(quest);

        // Actualizar la UI.
        questUI?.UpdateQuestUI();

        Debug.Log("Misión completada: " + quest.quest.questName);

    }

    public bool IsQuestHandedIn(string questId)
    {
        return handingQuestIDs.Contains(questId);
    }

    public bool RemoveRequiredItemsFromInventory(string questId)
    {
        QuestProgress quest = activeQuest.Find(q => q.QuestID == questId);

        if (quest == null || subscribedInventory == null) return false;


        Dictionary<ItemData, int> requiredItems = new();

        foreach (QuestObjectives objective in quest.objetives) 
        { 
            if (objective.type != ObjectiveType.CollecItem)
            {
                continue;
            }

            if (objective.requiredItem == null)
            {
                continue;
            }

            ItemData item = objective.requiredItem;

            if (requiredItems.ContainsKey(item))
            {
                requiredItems[item] += objective.requiredAmount;
            }
            else
            {
                requiredItems.Add(item, objective.requiredAmount);
            }
        }

       foreach(var requirement in requiredItems)
       {
            int amountInInventory = subscribedInventory.GetItemCount(requirement.Key);

            if (amountInInventory < requirement.Value)
            {
                return false;
            }

       }

        foreach(var requirement in requiredItems)
        {
            bool removed = subscribedInventory.RemoveItem(requirement.Key, requirement.Value);

            if (!removed)
            {
                Debug.Log("No se pudo sacar los items");
                return false;
            }

        }

        return true;
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
