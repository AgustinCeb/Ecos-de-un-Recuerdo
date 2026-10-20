using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }

    public List<QuestProgress> activeQuest = new();
    private QuestUI questUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        questUI = FindObjectOfType<QuestUI>();

    }

    public void AcceptQuest(QuestData quest)
    {
        if(IsQuestActive(quest.questId)) return;

        activeQuest.Add(new QuestProgress(quest));
        questUI.UpdateQuestUI();
    }

    public bool IsQuestActive(string questID) => activeQuest.Exists(q => q.QuestID == questID);
    
}
