using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Npc : MonoBehaviour, IInteractable
{
    public Npc_DialogeData dialogeData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, npcName;
    public Image portaitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    public string ActionText => "Hablar";

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
            dialogueText.SetText(dialogeData.dialogeLines[dialogueIndex]);
            isTyping = false;

        }
        else if (++dialogueIndex < dialogeData.dialogeLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }

    }

    void StartDialogue()
    {
        Debug.Log("StartDialogue() se ejecutó");

        isDialogueActive = true;
        dialogueIndex = 0;

        npcName.SetText(dialogeData.npcName);
        portaitImage.sprite = dialogeData.npcPortrait;

        dialoguePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in dialogeData.dialogeLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogeData.typingSpeed);

        }

        isTyping = false;

        if(dialogeData.autoProgressLines.Length > dialogueIndex && dialogeData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogeData.autoProgressDelay);

            NextLine();
        }

    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);

    }

}
