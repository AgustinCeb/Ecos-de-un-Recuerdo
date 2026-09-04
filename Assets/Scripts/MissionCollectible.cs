using System;
using UnityEngine;

public class MissionCollectible : MonoBehaviour, IInteractable 
{
    public static Action onMissionCollect;

    public string ActionText => "Cosechar";

    private void OnEnable()
    {
    }
    
    public void Interact(GameObject Starter)
    {
        if (Starter.transform.CompareTag("Player"))
        {
            onMissionCollect?.Invoke();
            Destroy(this.gameObject);
        }

    }
    

}
