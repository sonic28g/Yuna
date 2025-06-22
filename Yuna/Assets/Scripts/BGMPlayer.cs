using System.Linq;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer Instance { get; private set; }

    private AudioSource _audioSource;

    [Header("BGM Clips")]
    [SerializeField] private ClipByType[] _bgmClips;
    private BGMType? _lastType;


    private void Awake()
    {
        // Singleton pattern
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialization
        bool hasAudioSource = TryGetComponent(out _audioSource);
        if (!hasAudioSource) throw new System.Exception($"AudioSource component is missing on {name}.");
    }


    public void Play(BGMType? type = null)
    {
        // If no type is provided, use the last played type
        type ??= _lastType;

        // Set the last played type (or do nothing if null)
        if (type == null) return;
        _lastType = type;

        // Get candidates for the specified type (and ensure it has a clip)
        ClipByType[] candidates = _bgmClips
            .Where(c => c.Type == type && c.Clip != null)
            .ToArray();

        // Select a random clip from the candidates (or do nothing if none are available)
        if (candidates.Length == 0) return;
        ClipByType selected = candidates[Random.Range(0, candidates.Length)];
        var selectedClip = selected.Clip;

        // If the selected clip is the same as the currently playing one, do nothing
        if (selectedClip == _audioSource.clip && _audioSource.isPlaying) return;

        // Play the new clip
        Stop();
        _audioSource.clip = selectedClip;
        _audioSource.Play();
    }

    public void Stop()
    {
        if (_audioSource.isPlaying) _audioSource.Stop();
    }


    public void Muffle()


    public enum BGMType
    {
        // Cutscene + Main Menu
        MainMenu,

        // Game
        YunaHouse,
        Outside,
        Village,
        GuardHouse,
        Werehouse,
        Market,
        // ...
    }

    [System.Serializable]
    public class ClipByType
    {
        public BGMType Type;
        public AudioClip Clip;
    }
}
