using System;
using UnityEngine;

public class Door : InteractableObject
{
    public Animator animator; // Referência ao Animator da porta
    private bool isOpen = false; // Estado da porta

    [Header("Requisitos de tutorial")]
    public bool bloqueadaPorTutorial = true;
    public int etapaMinima = 2; // A partir de que etapa pode interagir
    public static event Action<Door> OnDoorOpened;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip[] _clips;
    private AudioSource _audioSource;


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
        // Verificar se pode interagir com base no progresso do tutorial
        if (bloqueadaPorTutorial && TutorialManager.Instance != null &&
            TutorialManager.Instance.currentIndex < etapaMinima)
        {
            UIManager.instance.ShowInteractionText("Finish the tutorial task first");
            // Aqui podes mostrar mensagem ao jogador na UI, se quiseres
            return;
        }

        if (!isOpen) OpenDoor();
        else CloseDoor();

        PlayOpenCloseSound();
    }

    private void OpenDoor()
    {
        isOpen = true;
        animator.SetTrigger("Open");

        OnDoorOpened?.Invoke(this);
    }

    private void CloseDoor()
    {
        isOpen = false;
        animator.SetTrigger("Close");
    }
}
