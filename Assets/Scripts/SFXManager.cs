using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    [SerializeField] private AudioSource AudioObj;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void PlaySFX(AudioClip audioClip, Transform spawnPos,float volume, float pitch)
    {
        AudioSource audioSource = Instantiate(AudioObj, spawnPos.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;    
        audioSource.pitch = pitch;

        audioSource.Play();

        float lenght = audioSource.clip.length;

        Destroy(audioSource.gameObject, lenght);
    }
}

