using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float runSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpPower;

    private CharacterController controller;
    private Animator animator;
    private Vector3 moveDir;
    private float ySpeed = 0;       // 중력 역할
    private float moveSpeed;
    private bool isWalking;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    void Move() 
   {
        // 월드기준 움직임
        //controller.Move(moveDir * runSpeed * Time.deltaTime);

        if (moveDir.magnitude <= 0)     // 움직이지 않을 경우
            moveSpeed = Mathf.Lerp(moveSpeed, 0, 1f);         // 선형보간, moveSpeed가 0을 목표로 0.5퍼센트씩 떨어짐

        else if (isWalking)             // 움직일 경우
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.5f);
        }

        else                            // 뛸경우
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.5f);
        }

        // 로컬기준 움직임
        controller.Move(transform.forward * moveDir.z * runSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * runSpeed * Time.deltaTime);

        // Mathf.Lerp() 애니메이션에 부드러운 전환을 넣기위한 정밀작업

        animator.SetFloat("XSpeed", moveDir.x, 0.1f, Time.deltaTime);
        animator.SetFloat("YSpeed", moveDir.z, 0.1f, Time.deltaTime);

        animator.SetFloat("Speed", moveSpeed);
    }

    void OnMove(InputValue value) 
    {
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x, 0 , input.y);
    }

    void Jump() 
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;   // 게임속중력을 더해줘서 중력가속도를 구현함

        /* 그라운드 체크가 기능이 있지만 정교함이 별로 쓸일이없음
        if(controller.isGrounded)
            ySpeed = 0;
        */
        if (GroundCheck() && ySpeed < 0)
            ySpeed = -1;

        controller.Move(Vector3.up * ySpeed*Time.deltaTime);
    }

    void OnJump(InputValue value) 
    {
        if (GroundCheck())
            ySpeed = jumpPower;
    }

    private bool GroundCheck() 
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f);
    }

    private void OnWalk(InputValue value) 
    {
        isWalking = value.isPressed;
    }

}
