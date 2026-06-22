using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private EventInstance _musicInstance;

    [Header("Mute")]
    [SerializeField] private bool _mute;

    private bool _lastMute;
    private Bus _masterBus;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Mute
        _masterBus = RuntimeManager.GetBus("bus:/Master Bus");
        _lastMute = !_mute;

        _musicInstance = RuntimeManager.CreateInstance("event:/Music_Town(Vertical)");

        _musicInstance.setParameterByName("Town_1", 0);

        _musicInstance.start();
    }

    private void Update()
    {
        if (_mute != _lastMute)
        {
            _masterBus.setMute(_mute);
            _lastMute = _mute;
        }
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
