using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public Transform questListConten;
    public GameObject questEntryPF;
    public GameObject objectiveTextPF;


    void Start()
    {
        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        foreach (Transform child in questListConten)
        {
            Destroy(child.gameObject);
        }

        foreach(var quest in QuestController.Instance.activeQuest)
        {
            GameObject entry = Instantiate(questEntryPF,questListConten);
            TMP_Text questNameText = entry.transform.Find("QuestName").GetComponent<TMP_Text>();
            Transform objectiveList = entry.transform.Find("QuestObjective");

            questNameText.text = quest.quest.questName;

            foreach(var objective in quest.objetives)
            {
                GameObject objTextGO = Instantiate(objectiveTextPF,objectiveList);
                TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                objText.text = $"{objective.description}({objective.currentAmount}/{objective.requiredAmount})";
            }

        }

    }
}
