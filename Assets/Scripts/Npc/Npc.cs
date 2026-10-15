using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Npc : MonoBehaviour, IInteractable
{
    public Npc_DialogeData dialogeData;
    public DialogueControler dialogueUi;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    public string ActionText => "Hablar";

    private void Start()
    {
        dialogueUi = DialogueControler.Instance;
    }

    public bool CantInteract()
    {
        return !isDialogueActive; 
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
        
        isDialogueActive = true;
        dialogueIndex = 0;

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

    void DisplayChoices(DialogueChoice choice)
    {
        for (int i = 0; i < choice.choices.Length; i++) 
        { 
            int nextIndex = choice.nextDialogueIndexes[i];
            dialogueUi.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex));
        }
    }

    void ChooseOption(int nextIndex)
    {
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
