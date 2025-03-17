using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public event Action OnDeath;
    public bool IsDead { get; private set; } = false;


    public void Kill()
    {
        if (IsDead) return;

        IsDead = true;
        OnDeath?.Invoke();
    }
}
