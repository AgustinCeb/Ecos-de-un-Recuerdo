using System;
using UnityEngine;

public class MissionCollectible : MonoBehaviour
{
    public static Action onMissionCollect;

    private void OnEnable()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            onMissionCollect?.Invoke();
            Destroy(this.gameObject);
        }
    }

}
