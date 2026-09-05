using System;
using UnityEngine;

public class MissionCollectible : MonoBehaviour, IInteractable 
{
    public static Action onMissionCollect;

    [SerializeField] private GameObject _emptyTreePF;

    public string ActionText => "Cosechar";

    private void OnEnable()
    {
    }
    
    public void Interact(GameObject Starter)
    {
        if (Starter.transform.CompareTag("Player"))
        {
            onMissionCollect?.Invoke();

            Instantiate(_emptyTreePF,transform.position,transform.rotation);

            Destroy(this.gameObject);
        }

    }
    

}
