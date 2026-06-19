using UnityEngine;

public class TownTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        MusicManager.Instance.SetTown(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        MusicManager.Instance.SetTown(false);
    }
}
