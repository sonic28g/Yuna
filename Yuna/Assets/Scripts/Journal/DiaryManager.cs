using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiaryManager : MonoBehaviour
{
    public static DiaryManager Instance { get; private set; }

    public TextMeshProUGUI diaryText;

    private bool noClues = true;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip _openClip;
    [SerializeField] private AudioClip _closeClip;
    [SerializeField] private AudioClip _tabClip;
    private AudioSource _audioSource;


    private void Start() {
        diaryText.text = "No clues yet...";
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _audioSource = GetComponent<AudioSource>();
    }

    public void UpdateDiary(InspectableData inspectableData)
    {
        if (noClues)
        {
            diaryText.text = "";
            noClues = false;
        }

        diaryText.text += inspectableData.inspectableTitle + ": " + inspectableData.inspectableDescription + "\n";
    
    }


    public void PlayOpenSound()
    {
        if (_audioSource == null || _openClip == null) return;
        _audioSource.PlayOneShot(_openClip);
    }

    public void PlayCloseSound()
    {
        if (_audioSource == null || _closeClip == null) return;
        _audioSource.PlayOneShot(_closeClip);
    }

    public void PlayTabSound()
    {
        if (_audioSource == null || _tabClip == null) return;
        _audioSource.PlayOneShot(_tabClip);
    }
}

