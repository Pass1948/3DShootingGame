using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;

    private CharacterController controller;
    private Vector3 moveDir;
    private float ySpeed = 0;       // 중력 역할

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

   void Move() 
   {
        // 월드기준 움직임
        // controller.Move(moveDir * moveSpeed * Time.deltaTime);

        // 로컬기준 움직임
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
    }

    void OnMove(InputValue value) 
    {
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x, 0 , input.y);

    }

    void Jump() 
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;

        /* 그라운드 체크가 기능이 있지만 정교함이 별로 쓸일이없음
        if(controller.isGrounded)
        {
            ySpeed = 0;
        }
        */
        if (GroundCheck() && ySpeed <0)
            ySpeed = 0;

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
}
