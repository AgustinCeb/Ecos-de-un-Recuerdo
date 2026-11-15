using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavedItem
{
    public int ItemID;
    public int Quantity;

    public SavedItem(int itemID, int quantity)
    {
        ItemID = itemID;
        Quantity = quantity;
    }
}

[Serializable]
public class PlayerData
{
    [Header("Progress")]
    public int PlayerLevel = 1;
    public int HeldXP = 0;

    [Header("Inventories")]
    public List<SavedItem> Inventory = new();
    public List<SavedItem> Inventory2 = new();
    public List<SavedItem> Inventory3 = new();

    [Header("Equipped Runes")]
    public int FeetRuneID = -1;
    public int HeadRuneID = -1;
    public int BodyRuneID = -1;

    [Header("Quests")]
    public List<string> ActiveQuestIDs = new();
    public List<string> CompletedQuestIDs = new();
    public List<string> HandingQuestIDs = new();

    [Header("Checkpoint")]
    public bool HasCheckpoint = false;
    public Vector3 CheckpointPosition;
    public Quaternion CheckpointRotation;

}