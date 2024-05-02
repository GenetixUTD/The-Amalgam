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

    public int currentFloor;

    public bool WalkingSound;
    public bool SprintingSound;

    [SerializeField] public bool[] unlockedLogs;

    [SerializeField] public bool[] unlockedFloors;

    private float InteractRange = 5.0f;
    private GameObject previousLook;

    public GameObject PauseMenu;

    [SerializeField]
    public Gun PlayerGun;

    private bool isCrouching;

    public float StandingHeight;
    public float CrouchingHeight;

    public float walkingFootstepFrequency;
    public float sprintingFootstepFrequency;
    public float footstepCounter;

    public GameObject hidingLocker;

    public AmalgamCentralAI amalgamReference;

    public GameObject qtescript;

    private void Start()
    {
        Controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ActiveState = PlayerState.Active;
        currentFloor = 2;
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
        else if(ActiveState == PlayerState.HidingGame)
        {
            PlayerGun.gameObject.SetActive(false);
            if(qtescript.GetComponent<QTEGame>().gameState == QTEGame.QTEState.Success)
            {
                ActiveState = PlayerState.Hiding;
            }
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
            float footstepFrequency;

            if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
            {
                WalkingSound = false;
                SprintingSound = true;
                footstepFrequency = sprintingFootstepFrequency;
            }
            else
            {
                WalkingSound = true;
                SprintingSound = false;
                footstepFrequency = walkingFootstepFrequency;
            }

            if(footstepCounter >= 1f/footstepFrequency)
            {
                AkSoundEngine.PostEvent("play_playerfootstepevent", this.gameObject);
                footstepCounter = 0;
            }

            footstepCounter += (SpeedActual * Time.deltaTime);
        }
        else
        {
            WalkingSound = false;
            SprintingSound = false;
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
            if(hit.transform.gameObject.tag == "HideLocker" && !amalgamReference.playerInSight)
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

    public void EnableQTE()
    {
        Debug.Log("Enabling QTE");
        qtescript.GetComponent<QTEGame>().gameStart();
    }

    public void DisableQTE() 
    {
        qtescript.GetComponent<QTEGame>().gameStop();
    }

    public void unlockLog(int logID)
    {
        unlockedLogs[logID] = true;
    }

    public void unlockFloor(int floorID)
    {
        unlockedFloors[floorID] = true;
    }
}
