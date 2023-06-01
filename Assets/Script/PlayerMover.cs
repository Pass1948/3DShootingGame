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
    private float ySpeed = 0;       // �߷� ����
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
        // ������� ������
        //controller.Move(moveDir * runSpeed * Time.deltaTime);

        if (moveDir.magnitude <= 0)     // �������� ���� ���
            moveSpeed = Mathf.Lerp(moveSpeed, 0, 1f);         // ��������, moveSpeed�� 0�� ��ǥ�� 0.5�ۼ�Ʈ�� ������

        else if (isWalking)             // ������ ���
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.5f);
        }

        else                            // �۰��
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.5f);
        }

        // ���ñ��� ������
        controller.Move(transform.forward * moveDir.z * runSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * runSpeed * Time.deltaTime);

        // Mathf.Lerp() �ִϸ��̼ǿ� �ε巯�� ��ȯ�� �ֱ����� �����۾�

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
        ySpeed += Physics.gravity.y * Time.deltaTime;   // ���Ӽ��߷��� �����༭ �߷°��ӵ��� ������

        /* �׶��� üũ�� ����� ������ �������� ���� �����̾���
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
