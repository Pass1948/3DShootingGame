using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    public TrailRenderer bulletTrail;

    [SerializeField] float reloadTime;
    [SerializeField] Rig aimRig;
    [SerializeField] WeaponHolder weaponHolder;
    private Animator animator;

    private bool isReloading;

    private void Awake()
    {
        // 유니티 기본기능으로 Resources 폴더에 있는 오브젝트를 불러오는게 가능하다
        // bulletTrail = Resources.Load<TrailRenderer>("Prefabs/BulletTrail");
        animator = GetComponent<Animator>();
    }

    private void OnReload(InputValue value) 
    {
        if (isReloading)
            return;

        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine() 
    {
        animator.SetTrigger("Reload");
        isReloading = true;
        aimRig.weight = 0.1f;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        aimRig.weight = 0.6f;
    }

    public void Fire()
    {
        weaponHolder.Fire();
        animator.SetTrigger("Fire");
    }
    private void OnFire(InputValue value)
    {
        if (isReloading)
            return;

        Fire();
    }
}
