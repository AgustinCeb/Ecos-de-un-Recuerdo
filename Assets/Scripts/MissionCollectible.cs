using System;
using UnityEngine;

public class MissionCollectible : MonoBehaviour
{
    public static Action onMissionCollect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onMissionCollect?.Invoke();
        }
    }
}
