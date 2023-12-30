using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charactercontroller : MonoBehaviour
{
    private Vector3 Velocity;
    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouseInput;
    private float xRot;

    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private CharacterController Controller;
    [Space]
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float SprintSpeed;
    private float SpeedActual;
    [SerializeField] private float Jumpforce;
    [SerializeField] private float Sensitivity;
    private float Gravity = 9.81f;

    public AudioClip WalkingSound;
    public AudioClip SpringingSound;

    public AudioSource AudioManager;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MovePlayerCamera();
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput);

        if(Input.GetKey(KeyCode.LeftShift))
        {
            SpeedActual = SprintSpeed;
        }
        else
        {
            SpeedActual = WalkSpeed;
        }

        if (Controller.isGrounded)
        {
            Velocity.y = -1f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Velocity.y = Jumpforce;
            }

        }
        else
        {
            Velocity.y += Gravity * -2f * Time.deltaTime;
        }

        Controller.Move(MoveVector * SpeedActual * Time.deltaTime);
        Controller.Move(Velocity * Time.deltaTime);
    }

    private void MovePlayerCamera()
    {
        xRot -= PlayerMouseInput.y * Sensitivity;
        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }
}
