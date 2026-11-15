using System.Collections.Generic;
using UnityEngine;

public class QuestDictionari : MonoBehaviour
{
    [Header("Todas las misiones del juego")]
    [SerializeField] private List<QuestData> quests = new();

    private Dictionary<string, QuestData> questDictionary = new();

    private void Awake()
    {
        questDictionary.Clear();

        foreach (QuestData quest in quests)
        {
            if (quest == null)
                continue;

            if (string.IsNullOrEmpty(quest.questId))
            {
                Debug.LogWarning("Hay una misión sin questId asignado.");
                continue;
            }

            if (questDictionary.ContainsKey(quest.questId))
            {
                Debug.LogWarning("ID de misión duplicado: " + quest.questId);
                continue;
            }

            questDictionary.Add(quest.questId, quest);
        }
    }

    public QuestData GetQuest(string questID)
    {
        if (string.IsNullOrEmpty(questID))
            return null;

        questDictionary.TryGetValue(questID, out QuestData quest);

        if (quest == null)
            Debug.LogWarning("No se encontró la misión: " + questID);

        return quest;
    }
}
