using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnReload(InputValue value) 
    {
        animator.SetTrigger("Reload");
    }

    private void OnFire(InputValue value)
    {
        animator.SetTrigger("Fire");
    }
}
