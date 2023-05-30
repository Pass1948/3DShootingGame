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
        xRocation -= lookDelta.y * mouseSensitivity * Time.deltaTime;       // 위아래는 -로 반대값으로 해야함
        xRocation = Mathf.Clamp(xRocation, -80f, 80f);                      // 최대, 최소를 설정해서 회전을 제한 시킨다
        
        cameraRoot.localRotation = Quaternion.Euler(xRocation, 0, 0);       // 카메라만 회전
        transform.localRotation = Quaternion.Euler(0, yRocation, 0);        // 몸통 회전
    }

    void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }
}
