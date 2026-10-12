using System;
using UnityEngine;

public class MissionCollectible : MonoBehaviour, IInteractable 
{
    public static Action onMissionCollect;

    public bool IsEmpty;

    [SerializeField] private GameObject _emptyTreePF;

    public string ActionText => "Cosechar";

    

    public bool CantInteract()
    {
        return !IsEmpty;
    }

    public void Interact()
    {
        
        void TakeOne(GameObject Starter)
        {
        if (Starter.transform.CompareTag("Player"))
        {
            onMissionCollect?.Invoke();

            Instantiate(_emptyTreePF,transform.position,transform.rotation);

            Destroy(this.gameObject);
        }

        }
    }


}
