using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Quests/Quest")]
public class QuestData : ScriptableObject
{
    public string questId;
    public string questName;
    public string description;
    public List<QuestObjectives> objetives;


}

[System.Serializable]
public class QuestObjectives
{
    public string objectiveID;
    public string description;
    public ObjectiveType type;
    public int requiredAmount;
    public int currentAmount;

    public bool IsCompleted => currentAmount >= requiredAmount;

}

public enum ObjectiveType {CollecItem,DefeatEnemy,Custom}

[System.Serializable]

public class QuestProgress
{
    public QuestData quest;
    public List<QuestObjectives> objetives;

    public QuestProgress(QuestData quest)
    {
        this.quest = quest;
        objetives = new List<QuestObjectives>();

        foreach (var obj in quest.objetives)
        {
            objetives.Add(new QuestObjectives
            {
                objectiveID = obj.objectiveID,
                description = obj.description,
                type = obj.type,
                requiredAmount = obj.requiredAmount,
                currentAmount = 0
            });

        }

    }
    public bool IsCompleted => objetives.TrueForAll(o => o.IsCompleted);

    public string QuestID => quest.questId;

}