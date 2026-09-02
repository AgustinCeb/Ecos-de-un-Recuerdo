using System;
using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    public static Action onStartMission;
    public static Action onEndMission;
    MeshRenderer myRenderer;
    Collider myCollider;
    bool triggered;

    private void Awake()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myCollider = GetComponent<Collider>();
    }
    private void OnEnable()
    {
        berrymission.onReturn += MissionReturn;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Player"))
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
    }
}
