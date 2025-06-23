using System.Collections;
using System.Collections.Generic;
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

    private readonly List<object> _mufflers = new();

    private Coroutine _muffleCoroutine;
    private Coroutine _waitCoroutine;


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
        else
        {
            _audioMixer = _audioSource.outputAudioMixerGroup.audioMixer;
            _audioMixer.SetFloat(_lowpassParam, _normalFrequency);
        }
    }


    public void Play(BGMType? type = null, bool forceAnother = false)
    {
        // If no type is provided, use the last played type
        type ??= _lastType;
        if (type == null) return;

        // If the type is the same as the last one, and we're not forcing another play, do nothing
        if (_lastType == type && !forceAnother && _audioSource.isPlaying) return;

        // Set the last type to the current one
        _lastType = type;

        // Get candidates for the specified type (and ensure it has a clip)
        ClipByType[] candidates = _bgmClips
            .Where(c => c.Type == type && c.Clip != null)
            .ToArray();

        // Select a random clip from the candidates (or do nothing if none are available)
        if (candidates.Length == 0) return;
        ClipByType selected = candidates[Random.Range(0, candidates.Length)];
        var selectedClip = selected.Clip;

        // Play the new clip
        Stop();
        _audioSource.clip = selectedClip;
        _audioSource.Play();

        // Start the wait coroutine to handle clip end
        _waitCoroutine = StartCoroutine(WaitForClipEnd(selectedClip.length));
    }


    public void Stop()
    {
        // Stop the wait coroutine if it's running
        if (_waitCoroutine != null) StopCoroutine(_waitCoroutine);
        _waitCoroutine = null;

        // Stop the audio source if it's playing
        if (!_audioSource.isPlaying) _audioSource.Stop();
    }

    private IEnumerator WaitForClipEnd(float clipLength)
    {
        // Wait for the clip to finish + Play another clip
        yield return new WaitForSecondsRealtime(clipLength);
        Play(null, true);
    }


    public void Muffle(object muffler)
    {
        if (_mufflers.Contains(muffler)) return;
       
        if (_mufflers.Count == 0) StartMuffleTransition(_muffleFrequency);
        _mufflers.Add(muffler);
    }

    public void Unmuffle(object muffler)
    {
        if (!_mufflers.Contains(muffler)) return;
        _mufflers.Remove(muffler);

        if (_mufflers.Count > 0) return;
        StartMuffleTransition(_normalFrequency);
    }


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
            elapsed += Time.unscaledDeltaTime;

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
