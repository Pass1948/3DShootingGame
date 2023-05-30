using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCameraController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity;
    [SerializeField] Transform cameraRoot;
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


    private void LateUpdate()
    {
        Look();
    }

    void Look()
    {
        yRocation += lookDelta.x * mouseSensitivity * Time.deltaTime;
        xRocation -= lookDelta.y * mouseSensitivity * Time.deltaTime;       // ���Ʒ��� -�� �ݴ밪���� �ؾ���
        xRocation = Mathf.Clamp(xRocation, -80f, 80f);                      // �ִ�, �ּҸ� �����ؼ� ȸ���� ���� ��Ų��
        
        cameraRoot.localRotation = Quaternion.Euler(xRocation, 0, 0);       // ī�޶� ȸ��
        transform.localRotation = Quaternion.Euler(0, yRocation, 0);        // ���� ȸ��
    }

    void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }
}
