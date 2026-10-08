using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public Transform questListConten;
    public GameObject questEntryPF;
    public GameObject objectiveTextPF;

    public QuestData testQuest;
    public int testQuestAmount;
    private List<QuestProgress> testQuests = new();

    private void Start()
    {
        for (int i = 0; i< testQuestAmount; i++)
        {
            testQuests.Add(new QuestProgress(testQuest));
        }
        
        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        foreach (Transform child in questListConten)
        {
            Destroy(child.gameObject);
        }

        foreach(var quest in testQuests)
        {
            GameObject entry = Instantiate(questEntryPF,questListConten);
            TMP_Text questNameText = entry.transform.Find("QuestNameText").GetComponent<TMP_Text>();
            Transform objectiveList = entry.transform.Find("ObjectiveList");

            questNameText.text = quest.quest.name;

            foreach(var objective in quest.objetives)
            {
                GameObject objTextGO = Instantiate(objectiveTextPF,objectiveList);
                TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                objText.text = $"{objective.description}({objective.currentAmount}/{objective.requiredAmount})";
            }

        }

    }
}
