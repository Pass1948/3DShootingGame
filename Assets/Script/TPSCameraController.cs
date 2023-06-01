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
        xRocation -= lookDelta.y * mouseSensitivity * Time.deltaTime;       // 위아래는 -로 반대값으로 해야함
        xRocation = Mathf.Clamp(xRocation, -80f, 80f);                      // 최대, 최소를 설정해서 회전을 제한 시킨다

        cameraRoot.rotation = Quaternion.Euler(xRocation, yRocation, 0);       // 카메라만 회전
        
    }
    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }

}
