using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCameraController : MonoBehaviour
{
    [SerializeField] Transform cameraRoot;

    [SerializeField] Transform aimTarget;
    [SerializeField] float lookDistance;
    [SerializeField] float mouseSensitivity;

    private Vector2 lookDelta;
    private float xRocation;
    private float yRocation;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        Rotate();
    }

    private void LateUpdate()
    {
        Look();
    }

    private void Rotate() 
    {
        Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;
        aimTarget.position = lookPoint;
        lookPoint.y = transform.position.y;
        transform.LookAt(lookPoint);
    }

    private void Look()
    {
        yRocation += lookDelta.x * mouseSensitivity * Time.deltaTime;
        xRocation -= lookDelta.y * mouseSensitivity * Time.deltaTime;       // ���Ʒ��� -�� �ݴ밪���� �ؾ���
        xRocation = Mathf.Clamp(xRocation, -80f, 80f);                      // �ִ�, �ּҸ� �����ؼ� ȸ���� ���� ��Ų��

        cameraRoot.rotation = Quaternion.Euler(xRocation, yRocation, 0);       // ī�޶� ȸ��
        
    }
    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }

}
