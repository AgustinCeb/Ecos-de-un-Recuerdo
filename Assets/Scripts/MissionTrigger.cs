using System;
using UnityEngine;

public class MissionTrigger : MonoBehaviour, IInteractable
{
    public static Action onStartMission;
    public static Action onEndMission;
    MeshRenderer myRenderer;
    Collider myCollider;
    bool triggered;

    public String ActionText { get; private set; } = "Aceptar Mision";

    private void Awake()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myCollider = GetComponent<Collider>();
    }
    private void OnEnable()
    {
        berrymission.onReturn += MissionReturn;
    }
    public void Interact(GameObject Starter)
    {
        if (Starter.transform.CompareTag("Player"))
        {
            if (triggered)
            {
                onEndMission?.Invoke();
                this.gameObject.SetActive(false);
                
            }
            else
            {
                onStartMission?.Invoke();
                triggered = true;
                ToogleVisuals(false);
                
            }

        }
    }
    void ToogleVisuals(bool x)
    {
        myRenderer.enabled = x;
        myCollider.enabled = x;
    }
    void MissionReturn()
    {
        ToogleVisuals(true);
        ActionText = "Entregar Objetos";

    }
}
