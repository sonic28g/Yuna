using System;
using UnityEngine;

public class Attack : InteractableObject
{
    private ThirdPersonShooterController thirdPersonShooterController;
    private void Start()
    {
        thirdPersonShooterController = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonShooterController>();
    }

    public override void Interact()
    {
        thirdPersonShooterController.isAttacking = true;
        gameObject.GetComponentInParent<EnemyHealth>().Kill();
        gameObject.SetActive(false);
    }
}
