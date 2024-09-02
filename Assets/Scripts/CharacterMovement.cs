using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    
    [SerializeField] private float speed;
    [SerializeField] private float cameraSpeed = 5f;
    private Rigidbody rb;

    [SerializeField] private Transform characterCamera;

    private float maxVerticalMovement = 45;

    Vector2 cameraRotation = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        CharacterMovementKeyboard();
        CameraMovement();
    }

    private void CharacterMovementKeyboard()
    {
        if (rb.velocity.magnitude > 4f) return;

        Vector3 dir = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            dir += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir -= transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dir -= transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir += transform.right;
        }

        dir = dir.normalized;

        rb.AddForce(dir * speed * 100 * Time.fixedDeltaTime);
    }

    private void CameraMovement()
    {
        Vector2 cameraDir = Mouse.current.delta.value;

        if (Time.time < .1f) return;

        cameraRotation.y += cameraDir.x * Time.fixedDeltaTime * cameraSpeed;
        cameraRotation.x += -cameraDir.y * Time.fixedDeltaTime * cameraSpeed;

        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -maxVerticalMovement, maxVerticalMovement);
        
        if(Mathf.Abs(cameraRotation.y) >= 360)
        {
            cameraRotation.x = 0;
        }

        Vector2 cameraRot = new Vector2(cameraRotation.x, 0);

        characterCamera.localEulerAngles = cameraRot;

        Vector3 characterRot = new Vector2(0, cameraRotation.y);

        transform.localEulerAngles = characterRot;
    }
}
