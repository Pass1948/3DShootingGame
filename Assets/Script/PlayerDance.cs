using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDance : MonoBehaviour
{
    private Animator animator;
    private bool isDance;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnDance(InputValue value)
    {
        isDance = value.isPressed;
        animator.SetLookAtWeight(2, 1f);
    }
}
