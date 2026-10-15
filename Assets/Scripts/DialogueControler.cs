using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControler : MonoBehaviour
{
    public static DialogueControler Instance { get; private set; }
    
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portaitImage;
    public Transform choiceContainer;
    public GameObject choiceButtonPF;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowDialogueUi(bool show)
    {
        dialoguePanel.SetActive(show);
    }

    public void SetNPCInfo(string npcName, Sprite portrait)
    {
        nameText.text = npcName;
        portaitImage.sprite = portrait;
    }

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void ClearChoices()
    {
        foreach (Transform child in choiceContainer) Destroy(child.gameObject);
    }

    public void CreateChoiceButton(string choiceText,UnityEngine.Events.UnityAction onClick)
    {
        GameObject ChoiceButton = Instantiate(choiceButtonPF,choiceContainer);
        ChoiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        ChoiceButton.GetComponent<Button>().onClick.AddListener(onClick);
    }
}
