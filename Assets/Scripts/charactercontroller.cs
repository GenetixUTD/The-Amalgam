using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charactercontroller : MonoBehaviour
{
    private Vector3 Velocity;
    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouseInput;
    private float xRot;

    public enum PlayerState
    {
        Active,
        Hiding,
        Paused,
        HidingGame
    }

    public PlayerState ActiveState;

    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private CharacterController Controller;
    [Space]
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float SprintSpeed;
    private float SpeedActual;
    [SerializeField] private float Jumpforce;
    [SerializeField] private float Sensitivity;
    private float Gravity = 9.81f;

    public AudioSource WalkingSound;
    public AudioSource SprintingSound;

    private float InteractRange = 5.0f;
    private GameObject previousLook;

    public GameObject PauseMenu;

    [SerializeField]
    public Gun PlayerGun;

    private bool isCrouching;

    public float StandingHeight;
    public float CrouchingHeight;

    public GameObject hidingLocker;

    private void Start()
    {
        Controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ActiveState = PlayerState.Active;
    }

    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if(ActiveState == PlayerState.Active)
        {
            PlayerGun.gameObject.SetActive(true);
            MovePlayer();
            MovePlayerCamera();
            FootstepSounds();
            PlayerInteract();
        }
        else if(ActiveState == PlayerState.Hiding)
        {
            PlayerGun.gameObject.SetActive(false);
            MovePlayerCamera();
        }
        else if(ActiveState == PlayerState.Paused)
        {
            PlayerGun.gameObject.SetActive(false);
            //Do Nothing.
        }
        
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput);

        if(Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            SpeedActual = SprintSpeed;
        }
        else
        {
            SpeedActual = WalkSpeed;
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            Controller.height = CrouchingHeight;
            isCrouching = true;
        }
        else
        {
            Controller.height = StandingHeight;
            isCrouching = false;
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
        xRot = Mathf.Clamp(xRot, -90, 90);
        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    private void FootstepSounds()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {

            if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
            {
                WalkingSound.enabled = false;
                SprintingSound.enabled = true;
            }
            else
            {
                WalkingSound.enabled = true;
                SprintingSound.enabled = false;
            }
        }
        else
        {
            WalkingSound.enabled = false;
            SprintingSound.enabled = false;
        }
    }

    private void PlayerInteract()
    {
        RaycastHit hit;
        Debug.DrawRay(PlayerCamera.position, 10* PlayerCamera.forward, Color.magenta);
        if(Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out hit, InteractRange))
        {
            if(hit.transform.gameObject.tag == "AmmoBox")
            {
                hit.transform.gameObject.GetComponent<ItemPickup>().EnableGlow();
                previousLook = hit.transform.gameObject;
                if(Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.gameObject.GetComponent<ItemPickup>().InteractedWith(this);
                }
                
            }
            if(hit.transform.gameObject.tag == "HideLocker")
            {
                hit.transform.gameObject.transform.GetComponentInParent<Locker>().EnableGlow();
                previousLook = hit.transform.gameObject;
                if(Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.gameObject.GetComponentInParent<Locker>().InteractedWith(this);
                    hidingLocker = hit.transform.parent.gameObject;
                }
            }

            
        }


        if (previousLook != null)
        {
            if (previousLook.tag == "AmmoBox")
            {
                if (!Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out hit, InteractRange))
                {
                    previousLook.GetComponent<ItemPickup>().DisableGlow();
                    previousLook = null;
                }
                else if (previousLook != hit.transform.gameObject)
                {
                    previousLook.GetComponent<ItemPickup>().DisableGlow();
                    previousLook = null;
                }
            }
            else if(previousLook.tag == "HideLocker")
            {
                if (!Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out hit, InteractRange))
                {
                    previousLook.GetComponentInParent<Locker>().DisableGlow();
                    previousLook = null;
                }
                else if (previousLook != hit.transform.gameObject)
                {
                    previousLook.GetComponentInParent<Locker>().DisableGlow();
                    previousLook = null;
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            this.ActiveState = PlayerState.Paused;
        }
    }
}
