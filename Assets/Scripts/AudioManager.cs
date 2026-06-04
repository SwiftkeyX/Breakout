using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] public AudioClip SfxHitBrick;
    [SerializeField] public AudioClip SfxBrickBreak;
    [SerializeField] public AudioClip SfxHitPaddle;
    [SerializeField] public AudioClip SfxHitWall;
    [SerializeField] public AudioClip SfxBallLost;
    [SerializeField] public AudioClip SfxLevelClear;
    [SerializeField] public AudioClip SfxPowerUp;
    [SerializeField] public AudioClip MusicLoop;

    [SerializeField] private float _sfxVolume   = 0.8f;
    [SerializeField] private float _musicVolume = 0.4f;

    private AudioSource[] _sfxPool;
    private AudioSource _musicSource;
    private int _poolIndex;
    private BrickManager _brickManager;

    private const int POOL_SIZE = 6;

    void Awake()
    {
        if (Instance != null) return;
        Instance = this;
        _sfxPool = new AudioSource[POOL_SIZE];
        for (int i = 0; i < POOL_SIZE; i++)
        {
            _sfxPool[i] = gameObject.AddComponent<AudioSource>();
            _sfxPool[i].playOnAwake = false;
            _sfxPool[i].volume = _sfxVolume;
        }
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.loop = true;
        _musicSource.volume = _musicVolume;
        _musicSource.playOnAwake = false;
    }

    void Start()
    {
        if (Instance != this) return;
        SceneManager.sceneLoaded   += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        TrySubscribeBrickManager();
        PlayMusic();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded   -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        UnsubscribeBrickManager();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TrySubscribeBrickManager();
        PlayMusic();
    }

    private void OnSceneUnloaded(Scene scene) => UnsubscribeBrickManager();

    private void TrySubscribeBrickManager()
    {
        UnsubscribeBrickManager();
        _brickManager = BrickManager.Instance;
        if (_brickManager == null) return;
        _brickManager.BrickDestroyed += OnBrickDestroyed;
        _brickManager.BrickDamaged   += OnBrickDamaged;
        _brickManager.LevelCleared   += OnLevelCleared;
    }

    private void UnsubscribeBrickManager()
    {
        if (_brickManager == null) return;
        _brickManager.BrickDestroyed -= OnBrickDestroyed;
        _brickManager.BrickDamaged   -= OnBrickDamaged;
        _brickManager.LevelCleared   -= OnLevelCleared;
        _brickManager = null;
    }

    private void OnBrickDestroyed(Vector3 _, Color __) => Play(SfxBrickBreak);
    private void OnBrickDamaged()                      => Play(SfxHitBrick);
    private void OnLevelCleared()                      => Play(SfxLevelClear);

    public void Play(AudioClip clip)
    {
        if (clip == null || _sfxPool == null) return;
        var src = _sfxPool[_poolIndex];
        if (src == null) return;
        src.clip = clip;
        src.Play();
        _poolIndex = (_poolIndex + 1) % POOL_SIZE;
    }

    public void PlayMusic()
    {
        if (MusicLoop == null || _musicSource == null || _musicSource.isPlaying) return;
        _musicSource.clip = MusicLoop;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        if (_musicSource != null) _musicSource.Stop();
    }
}
