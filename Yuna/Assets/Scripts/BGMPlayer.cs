using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer Instance { get; private set; }

    private AudioSource _audioSource;
    private AudioMixer _audioMixer;

    [Header("BGM Clips")]
    [SerializeField] private ClipByType[] _bgmClips;
    private BGMType? _lastType;

    [Header("Muffle Settings")]
    [SerializeField] private string _lowpassParam = "LowpassFrequency";

    [SerializeField] private float _muffleFrequency = 500f;
    [SerializeField] private float _normalFrequency = 22000f;
    [SerializeField] private float _muffleTransitionTime = 1f;

    private Coroutine _muffleCoroutine;


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

        if (_audioSource.outputAudioMixerGroup == null) Debug.LogWarning($"OutputAudioMixerGroup is not set for {name}. Muffle will not work.");
        else _audioMixer = _audioSource.outputAudioMixerGroup.audioMixer;
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


    public void Muffle() => StartMuffleTransition(_muffleFrequency);
    public void Unmuffle() => StartMuffleTransition(_normalFrequency);

    private void StartMuffleTransition(float targetFrequency)
    {
        if (_muffleCoroutine != null) StopCoroutine(_muffleCoroutine);
        _muffleCoroutine = StartCoroutine(MuffleTransition(targetFrequency));
    }

    private IEnumerator MuffleTransition(float targetFrequency)
    {
        // Get the initial lowpass filter frequency
        _audioMixer.GetFloat(_lowpassParam, out float startFreq);

        float elapsed = 0f;
        while (elapsed < _muffleTransitionTime)
        {
            elapsed += Time.deltaTime;

            // Calculate the interpolation factor and the current frequency
            float t = Mathf.Clamp01(elapsed / _muffleTransitionTime);
            float currentFreq = Mathf.Lerp(startFreq, targetFrequency, t);

            // Set the lowpass filter frequency
            _audioMixer.SetFloat(_lowpassParam, currentFreq);
            yield return null;
        }

        _audioMixer.SetFloat(_lowpassParam, targetFrequency);
    }


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
