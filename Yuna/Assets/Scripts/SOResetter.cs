using System.Collections.Generic;
using UnityEngine;

public interface IResettable
{
    void ResetState();
}

public class SOResetter : MonoBehaviour
{
    [SerializeField] private List<ScriptableObject> objectsToReset;

    public void ResetAll()
    {
        foreach (var so in objectsToReset)
        {
            if (so is IResettable resettable)
            {
                resettable.ResetState();
            }
        }
    }
}
