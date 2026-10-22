using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Npc : MonoBehaviour, IInteractable, IPlayerInteractable
{
    public Npc_DialogeData dialogeData;
    public DialogueControler dialogueUi;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    private enum QuestState { NotStartet, InProgress, Completed };
    private QuestState questState = QuestState.NotStartet;

    private Inventory2 interactingInventory;

    public string ActionText => "Hablar";

    private void Start()
    {
        dialogueUi = DialogueControler.Instance;
    }

    public bool CantInteract()
    {
        return !isDialogueActive; 
    }

    public void interact(PlayerInteract player)
    {
        interactingInventory = player.GetComponent<Inventory2>();

        if (interactingInventory == null)
        {
            Debug.Log("El jugador no tiene Inventario");
            return;
        }

        Interact();
    }

    public void Interact()
    {
        if (dialogeData == null) return;
        

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUi.SetDialogueText(dialogeData.dialogeLines[dialogueIndex]);
            isTyping = false;
            return;
        }

        dialogueUi.ClearChoices();

        if (dialogeData.endDialogueLines.Length > dialogueIndex && dialogeData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }

        foreach (DialogueChoice dialogueChoice in dialogeData.choices)
        {
            if(dialogueChoice.dialogueIndex == dialogueIndex)
            {
                DisplayChoices(dialogueChoice);
                return;
            }
        }

        if (++dialogueIndex < dialogeData.dialogeLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }

    }

    void StartDialogue()
    {
        //synq quest data
        SyncQuestState();

        //dialogos si hay quest activo
        if (questState == QuestState.NotStartet)
        {
            dialogueIndex = 0;
        }
        else if(questState == QuestState.InProgress)
        {
            dialogueIndex = dialogeData.questInProgressIndex;
        }
        else if (questState == QuestState.Completed)
        {
            dialogueIndex = dialogeData.questCompletedIndex;
        }

        isDialogueActive = true;
       

        dialogueUi.SetNPCInfo(dialogeData.npcName, dialogeData.npcPortrait);
        dialogueUi.ShowDialogueUi(true);

        DisplayCurrentLine();
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUi.SetDialogueText("");

        foreach (char letter in dialogeData.dialogeLines[dialogueIndex])
        {
            dialogueUi.SetDialogueText(dialogueUi.dialogueText.text += letter);
            yield return new WaitForSeconds(dialogeData.typingSpeed);

        }

        isTyping = false;

        if(dialogeData.autoProgressLines.Length > dialogueIndex && dialogeData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogeData.autoProgressDelay);

            NextLine();
        }

    }

    private void SyncQuestState()
    {
        if (dialogeData.quest == null) return;

        string questID = dialogeData.quest.questId;

        if (QuestController.Instance.IsQuestActive(questID))
        {
            questState= QuestState.InProgress;
        }
        else
        {
            questState = QuestState.NotStartet;
        }
    }

    void DisplayChoices(DialogueChoice choice)
    {
        for (int i = 0; i < choice.choices.Length; i++) 
        { 
            int nextIndex = choice.nextDialogueIndexes[i];
            bool giveQuest = choice.givesQuest[i];
            dialogueUi.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex, giveQuest));
        }
    }

    void ChooseOption(int nextIndex, bool giveQuest)
    {
        if (giveQuest)
        {
            QuestController.Instance.AcceptQuest(dialogeData.quest,interactingInventory);
            questState = QuestState.InProgress;
        }

        dialogueIndex = nextIndex;
        dialogueUi.ClearChoices();
        DisplayCurrentLine();
    }

    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueUi.SetDialogueText("");
        dialogueUi.ShowDialogueUi(false);

    }

}
