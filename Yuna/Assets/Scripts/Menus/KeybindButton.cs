using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeybindButton : MonoBehaviour
{
    public InputActionReference actionReference;
    public string compositePartName; // Deixa vazio ("") para bindings normais
    public TextMeshProUGUI buttonText;
    public Button button;

    private int bindingIndex = -1;
    private bool waitingForInput = false;

    void Start()
    {
        var bindings = actionReference.action.bindings;

        if (string.IsNullOrEmpty(compositePartName))
        {
            // Procurar binding normal (não é parte de composite)
            for (int i = 0; i < bindings.Count; i++)
            {
                if (!bindings[i].isPartOfComposite && !bindings[i].isComposite)
                {
                    bindingIndex = i;
                    break;
                }
            }
        }
        else
        {
            // Procurar binding que seja parte do composite com o nome indicado
            for (int i = 0; i < bindings.Count; i++)
            {
                if (bindings[i].isPartOfComposite &&
                    bindings[i].name.ToLower() == compositePartName.ToLower())
                {
                    bindingIndex = i;
                    break;
                }
            }
        }

        if (bindingIndex == -1)
        {
            Debug.LogWarning($"[KeybindButton] Binding não encontrado para: '{compositePartName}'");
        }

        UpdateButtonText();
    }

    void UpdateButtonText()
    {
        if (bindingIndex >= 0)
        {
            buttonText.text = actionReference.action.GetBindingDisplayString(bindingIndex,
                InputBinding.DisplayStringOptions.DontIncludeInteractions |
                InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
        }
    }

    public void StartRebinding()
    {
        if (waitingForInput || bindingIndex < 0)
            return;

        waitingForInput = true;
        buttonText.text = "Pressiona uma tecla...";

        actionReference.action.Disable();

        actionReference.action.PerformInteractiveRebinding(bindingIndex)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(op =>
            {
                op.Dispose();
                waitingForInput = false;
                actionReference.action.Enable();
                UpdateButtonText();
            })
            .OnCancel(op =>
            {
                op.Dispose();
                waitingForInput = false;
                actionReference.action.Enable();
                UpdateButtonText();
            })
            .Start();
    }
}
