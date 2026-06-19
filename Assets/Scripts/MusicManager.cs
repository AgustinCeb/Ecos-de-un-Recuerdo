using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private EventInstance _musicInstance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _musicInstance = RuntimeManager.CreateInstance("event:/Music_Town(Vertical)");

        _musicInstance.setParameterByName("Town_1", 0);

        _musicInstance.start();
    }

    public void SetTown(bool inTown)
    {
        _musicInstance.setParameterByName("Town_1", inTown ? 1 : 0);
    }

    private void OnDestroy()
    {
        _musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _musicInstance.release();
    }

}
