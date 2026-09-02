using System;
using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    public static Action onStartMission;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            onStartMission?.Invoke();
            this.enabled = false;
        }
    }
}
