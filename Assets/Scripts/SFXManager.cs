using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SFXManager : NetworkBehaviour
{
    public static SFXManager instance;

    [System.Serializable]
    private class SFXEntry
    {
        public string id;
        public AudioClip clip;
    }

    [Header("Audio")]
    [SerializeField] private AudioSource _audioPrefab;
    [SerializeField] private List<SFXEntry> _sfxEntries = new List<SFXEntry>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        if (instance == this)
            instance = null;
    }

    public void PlaySFX(AudioClip audioClip, Transform spawnPos, float volume, float pitch)
    {
        if (audioClip == null || spawnPos == null)
            return;

        if (!IsSpawned)
        {
            Debug.LogWarning("SFXManager todavía no está iniciado en la red.");
            return;
        }

        NetworkObject actor = spawnPos.GetComponentInParent<NetworkObject>();

        if (actor == null)
        {
            Debug.LogWarning("No se encontró un NetworkObject en el jugador.");
            return;
        }

        if (!actor.IsOwner)
            return;

        string soundId = GetSFXId(audioClip);

        if (string.IsNullOrEmpty(soundId))
        {
            Debug.LogWarning($"El sonido {audioClip.name} no está registrado en SFXManager.");
            return;
        }

        RequestSFXRpc(
            soundId,
            new NetworkObjectReference(actor),
            volume,
            pitch
        );
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    private void RequestSFXRpc(
        string soundId,
        NetworkObjectReference actorReference,
        float volume,
        float pitch,
        RpcParams rpcParams = default)
    {
        if (!actorReference.TryGet(out NetworkObject actor))
            return;

        if (actor.OwnerClientId != rpcParams.Receive.SenderClientId)
            return;

        PlaySFXRpc(soundId, actor.transform.position, volume, pitch);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void PlaySFXRpc(string soundId, Vector3 position, float volume, float pitch)
    {
        AudioClip clip = GetSFXClip(soundId);

        if (clip == null || _audioPrefab == null)
            return;

        AudioSource audioSource = Instantiate(
            _audioPrefab,
            position,
            Quaternion.identity
        );

        audioSource.clip = clip;
        audioSource.volume = Mathf.Clamp01(volume);
        audioSource.pitch = Mathf.Clamp(pitch, 0.1f, 3f);

        // Sonido 3D.
        audioSource.spatialBlend = 1f;

        audioSource.Play();

        Destroy(audioSource.gameObject, clip.length / audioSource.pitch);
    }

    private string GetSFXId(AudioClip clip)
    {
        foreach (SFXEntry entry in _sfxEntries)
        {
            if (entry.clip == clip)
                return entry.id;
        }

        return null;
    }

    private AudioClip GetSFXClip(string id)
    {
        foreach (SFXEntry entry in _sfxEntries)
        {
            if (entry.id == id)
                return entry.clip;
        }

        return null;
    }

}

