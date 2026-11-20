using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicControler : MonoBehaviour
{
    public static MusicControler Instance;

    [Header("Canciones")]
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameMusic;

    [Header("Configuración")]
    [Range(0f, 1f)]
    [SerializeField] private float _volume = 0.3f;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
        _audioSource.volume = _volume;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    private void Start()
    {
        ChangeMusic(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeMusic(scene.name);
    }

    private void ChangeMusic(string sceneName)
    {
        AudioClip newMusic = null;

        if (sceneName == "Menu")
            newMusic = _menuMusic;
        else if (sceneName == "Game")
            newMusic = _gameMusic;

        if (newMusic == null)
        {
            _audioSource.Stop();
            return;
        }

        if (_audioSource.clip == newMusic && _audioSource.isPlaying)
            return;

        _audioSource.clip = newMusic;
        _audioSource.Play();
    }
}
