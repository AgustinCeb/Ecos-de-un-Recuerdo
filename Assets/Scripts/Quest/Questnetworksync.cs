using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Questnetworksync : NetworkBehaviour
{
    private bool _restoreRequested;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        if (PlayerDataManager.Instance == null)
        {
            Debug.LogWarning("No se encontró PlayerDataManager al aparecer el jugador.");
            return;
        }

        PlayerData data = PlayerDataManager.Instance.GetOrCreateData(OwnerClientId);

        if (data == null) return;

        RestoreQuestStateRpc(new QuestIDList(data.ActiveQuestIDs.ToArray()),new QuestIDList(data.CompletedQuestIDs.ToArray()));
    }

    [Rpc(SendTo.Owner)]
    private void RestoreQuestStateRpc(QuestIDList activeIDs,QuestIDList completedIDs)
    {
        if (_restoreRequested) return;

        _restoreRequested = true;

        StartCoroutine(WaitForQuestReferences(activeIDs, completedIDs));
    }

    private IEnumerator WaitForQuestReferences(QuestIDList activeIDs, QuestIDList completedIDs)
    {

        QuestController questController = null;
        QuestDictionari dictionary = null;
        Inventory2 inventory = GetComponent<Inventory2>();

        while (questController == null || dictionary == null)
        {
            questController = QuestController.Instance;
            dictionary = FindFirstObjectByType<QuestDictionari>();

            yield return null;
        }
        if (inventory == null)
        {
            Debug.LogError("No se encontró Inventory2 en el jugador.");
            yield break;
        }


        questController.RestoreQuestState(new List<string>(activeIDs.IDs),new List<string>(completedIDs.IDs),inventory,dictionary);

        Debug.Log("Misiones restauradas correctamente para el jugador: " + OwnerClientId);
    }

    public void SyncQuest(string questID, bool isActive, bool isCompleted)
    {
        if (!IsOwner) return;
        if (string.IsNullOrEmpty(questID)) return;

        SyncQuestRpc(questID, isActive, isCompleted);
    }

    [Rpc(SendTo.Server)]
    private void SyncQuestRpc(string questID, bool isActive, bool isCompleted)
    {
        if (PlayerDataManager.Instance == null)
        {
            Debug.LogWarning("No se encontró PlayerDataManager.");
            return;
        }

        PlayerData data = PlayerDataManager.Instance.GetOrCreateData(OwnerClientId);

        if (data == null) return;

        data.ActiveQuestIDs.Remove(questID);
        data.CompletedQuestIDs.Remove(questID);

        if (isActive)
            data.ActiveQuestIDs.Add(questID);

        if (isCompleted)
            data.CompletedQuestIDs.Add(questID);

        Debug.Log("Misión sincronizada en el servidor: " + questID);
    }
}
