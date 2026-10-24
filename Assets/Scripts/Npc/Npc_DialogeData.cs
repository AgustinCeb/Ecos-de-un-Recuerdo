using UnityEngine;


[CreateAssetMenu(fileName = "NewNpcDialogue", menuName ="Npc Dialogue")]
public class Npc_DialogeData : ScriptableObject
{
    [Header("Npc Data")]
    public string npcName;
    public Sprite npcPortrait;

    [Header("Npc Dialogues")]
    public string[] dialogeLines;
    public bool[] autoProgressLines;
    public bool[] endDialogueLines;

    [Header("Npc Choises")]
    public DialogueChoice[] choices;

    [Header("Dialogues Config")]
    public float typingSpeed = 0.05f;
    public float autoProgressDelay = 1.5f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;

    [Header("Quest Options")]
    public int questInProgressIndex;
    public int questCompletedIndex;
    public int principalQuestComplete;
    public QuestData quest;

}
[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex;
    public string[] choices;
    public int[] nextDialogueIndexes;
    public bool[] givesQuest;
}
