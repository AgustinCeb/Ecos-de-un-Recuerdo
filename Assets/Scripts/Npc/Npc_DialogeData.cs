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

    [Header("Dialogues Config")]
    public float typingSpeed = 0.05f;
    public float autoProgressDelay = 1.5f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;

}
