using System.Collections;
using System;
using UnityEngine;

public class Door : InteractableObject
{
    [Header("Posições Porta")]
    [SerializeField] Transform openDoorTransform;
    [SerializeField] Transform closedDoorTransform;

    [SerializeField] float doorMoveDuration = 1f; // Duração do movimento da porta

    [Header("Requisitos de Tutorial")]
    [SerializeField] bool bloqueadaPorTutorial = true;
    [SerializeField] int etapaMinima = 2;
    [SerializeField] static event Action<Door> OnDoorOpened;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip[] _clips;
    private AudioSource _audioSource;

    private bool isOpen = false;
    private bool isMoving = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void PlayOpenCloseSound()
    {
        if (_audioSource == null || _clips.Length == 0) return;

        int index = UnityEngine.Random.Range(0, _clips.Length);
        _audioSource.clip = _clips[index];
        _audioSource.Play();
    }

    public override void Interact()
    {
        if (bloqueadaPorTutorial && TutorialManager.Instance != null &&
            TutorialManager.Instance.currentIndex < etapaMinima)
        {
            UIManager.instance.ShowInteractionText("Finish the tutorial task first");
            return;
        }

        if (isMoving) return; // Impede interação durante movimento

        if (!isOpen) StartCoroutine(MoveDoor(openDoorTransform));
        else StartCoroutine(MoveDoor(closedDoorTransform));

        PlayOpenCloseSound();
    }

    private IEnumerator MoveDoor(Transform target)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < doorMoveDuration)
        {
            transform.position = Vector3.Lerp(startPos, target.position, elapsed / doorMoveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target.position;

        isOpen = (target == openDoorTransform);
        if (isOpen)
            OnDoorOpened?.Invoke(this);

        isMoving = false;
    }


    public void CutsceneInteractDoor()
    {
        if (!isOpen) StartCoroutine(MoveDoor(openDoorTransform));
        else StartCoroutine(MoveDoor(closedDoorTransform));
    }
}
