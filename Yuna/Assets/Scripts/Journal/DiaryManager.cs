using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiaryManager : MonoBehaviour
{
    public static DiaryManager Instance { get; private set; }

    public TextMeshProUGUI diaryText1;
    public TextMeshProUGUI diaryText2;

    private readonly List<InspectableData> _clues = new();

    [Header("Sound Settings")]
    [SerializeField] private AudioClip _openClip;
    [SerializeField] private AudioClip _closeClip;
    [SerializeField] private AudioClip _tabClip;
    private AudioSource _audioSource;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        diaryText1.text = "No clues yet...";
        _audioSource = GetComponent<AudioSource>();
    }

    public void UpdateDiary(InspectableData inspectableData)
    {
        if (inspectableData == null) return;
        if (_clues.Contains(inspectableData)) return;

        if (_clues.Count == 0)
            diaryText1.text = "";


        if (_clues.Count < 2)
        {
            diaryText1.text += inspectableData.inspectableTitle + ": " + inspectableData.inspectableDescription + "\n";
        }
        else if (_clues.Count >= 2)
        {
            diaryText2.text += inspectableData.inspectableTitle + ": " + inspectableData.inspectableDescription + "\n";
        }
        
        _clues.Add(inspectableData);

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

