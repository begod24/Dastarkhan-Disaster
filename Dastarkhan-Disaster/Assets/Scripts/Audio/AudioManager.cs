using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private SfxLibrarySO _library;
    [SerializeField] private AudioSource _sfxSource2D;
    [SerializeField] private int _voicePoolSize = 6;
    [SerializeField] private bool _persistAcrossScenes = true;

    private AudioSource[] _voicePool;
    private int _voiceCursor;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        if (_persistAcrossScenes) DontDestroyOnLoad(gameObject);

        if (_sfxSource2D == null)
        {
            _sfxSource2D = gameObject.AddComponent<AudioSource>();
            _sfxSource2D.playOnAwake = false;
            _sfxSource2D.spatialBlend = 0f;
        }

        BuildVoicePool();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerCarryChangedEvent>(OnCarryChanged);
        EventBus.Subscribe<StationItemPlacedEvent>(OnStationPlaced);
        EventBus.Subscribe<StationItemTakenEvent>(OnStationTaken);
        EventBus.Subscribe<StationItemReadyEvent>(OnStationReady);
        EventBus.Subscribe<StationItemBurnedEvent>(OnStationBurned);
        EventBus.Subscribe<OrderSpawnedEvent>(OnOrderSpawned);
        EventBus.Subscribe<OrderCompletedEvent>(OnOrderCompleted);
        EventBus.Subscribe<OrderExpiredEvent>(OnOrderExpired);
        EventBus.Subscribe<SessionEndedEvent>(OnSessionEnded);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerCarryChangedEvent>(OnCarryChanged);
        EventBus.Unsubscribe<StationItemPlacedEvent>(OnStationPlaced);
        EventBus.Unsubscribe<StationItemTakenEvent>(OnStationTaken);
        EventBus.Unsubscribe<StationItemReadyEvent>(OnStationReady);
        EventBus.Unsubscribe<StationItemBurnedEvent>(OnStationBurned);
        EventBus.Unsubscribe<OrderSpawnedEvent>(OnOrderSpawned);
        EventBus.Unsubscribe<OrderCompletedEvent>(OnOrderCompleted);
        EventBus.Unsubscribe<OrderExpiredEvent>(OnOrderExpired);
        EventBus.Unsubscribe<SessionEndedEvent>(OnSessionEnded);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void BuildVoicePool()
    {
        _voicePool = new AudioSource[_voicePoolSize];
        for (int i = 0; i < _voicePoolSize; i++)
        {
            var go = new GameObject($"SFXVoice_{i}");
            go.transform.SetParent(transform);
            var src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.spatialBlend = 0f;
            _voicePool[i] = src;
        }
    }

    public void Play2D(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null || _library == null) return;
        var voice = _voicePool[_voiceCursor];
        _voiceCursor = (_voiceCursor + 1) % _voicePool.Length;
        voice.pitch = pitch;
        voice.PlayOneShot(clip, volume * _library.MasterSfxVolume);
    }

    public void PlayButtonClick()
    {
        if (_library != null) Play2D(_library.ButtonClick);
    }

    public void PlaySessionStart()
    {
        if (_library != null) Play2D(_library.SessionStart);
    }

    private void OnCarryChanged(PlayerCarryChangedEvent e)
    {
        if (_library == null) return;
        if (e.Carried != null) Play2D(_library.Pickup);
        else Play2D(_library.Drop, 0.7f);
    }

    private void OnStationPlaced(StationItemPlacedEvent e)
    {
        if (_library != null) Play2D(_library.StationPlace);
    }

    private void OnStationTaken(StationItemTakenEvent e)
    {
        if (_library != null) Play2D(_library.StationTake, 0.8f);
    }

    private void OnStationReady(StationItemReadyEvent e)
    {
        if (_library != null) Play2D(_library.StationReady);
    }

    private void OnStationBurned(StationItemBurnedEvent e)
    {
        if (_library != null) Play2D(_library.StationBurned);
    }

    private void OnOrderSpawned(OrderSpawnedEvent e)
    {
        if (_library != null) Play2D(_library.OrderSpawned, 0.6f);
    }

    private void OnOrderCompleted(OrderCompletedEvent e)
    {
        if (_library != null) Play2D(_library.OrderDelivered, 1f, 1.05f);
    }

    private void OnOrderExpired(OrderExpiredEvent e)
    {
        if (_library != null) Play2D(_library.OrderExpired, 0.9f);
    }

    private void OnSessionEnded(SessionEndedEvent e)
    {
        if (_library != null) Play2D(_library.SessionEnd);
    }
}
